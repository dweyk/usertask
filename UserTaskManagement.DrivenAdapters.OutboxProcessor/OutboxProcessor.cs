using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserTaskManagement.Application;
using UserTaskManagement.Application.DrivenPorts.DomainModel;
using UserTaskManagement.Application.DrivenPorts.MessageBrokerPort;

namespace UserTaskManagement.DrivenAdapters.OutboxProcessor;

/// <summary>
/// Фоновый сервис для обработки сообщений Outbox и отправки их в брокер сообщений
/// </summary>
public partial class OutboxProcessorService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMessageBrokerPort _messageBroker;
    private readonly ILogger<OutboxProcessorService> _logger;
    private readonly OutboxProcessorOptions _options;

    public OutboxProcessorService(
        IServiceScopeFactory scopeFactory,
        IMessageBrokerPort messageBroker,
        IOptions<OutboxProcessorOptions> options,
        ILogger<OutboxProcessorService> logger
    )
    {
        _scopeFactory = scopeFactory;
        _messageBroker = messageBroker;
        _logger = logger;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        LogServiceStarted(_logger, _options.PollIntervalSeconds);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessOutboxAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                LogServiceStopping(_logger);
                break;
            }
            catch (Exception ex)
            {
                LogProcessingError(_logger, ex);
            }

            try
            {
                await Task.Delay(
                    TimeSpan.FromSeconds(_options.PollIntervalSeconds),
                    stoppingToken
                );
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
        }

        LogServiceStopped(_logger);
    }

    private async Task ProcessOutboxAsync(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var messages = await outboxRepository.GetUnprocessed(
            _options.BatchSize,
            ct
        );

        if (messages.Count == 0)
        {
            return;
        }

        foreach (var message in messages)
        {
            try
            {
                var topic = GetTopicForMessageType(message.Type);

                var key = message.Id.ToString();

                await _messageBroker.PublishAsync(
                    topic,
                    key,
                    message.Data,
                    ct
                );

                await outboxRepository.MarkAsProcessed(
                    message.Id,
                    ct
                );
            }
            catch (ArgumentOutOfRangeException ex)
            {
                LogUnknownMessageType(_logger, ex, message.Type);
            }
            catch (Exception ex)
            {
                LogPublishError(_logger, ex, message.Id, message.RetryCount);

                await outboxRepository.IncrementRetryCount(
                    message.Id,
                    ct
                );

                if (message.RetryCount >= _options.MaxRetryCount)
                {
                    LogMaxRetryExceeded(_logger, message.Id, _options.MaxRetryCount);
                }
            }
        }

        await unitOfWork.SaveChanges(ct);
    }

    /// <summary>
    /// Маппинг типа события на топик
    /// </summary>
    private string GetTopicForMessageType(string messageType)
        => messageType switch
        {
            "UserTaskCreatedEvent" => "user-task",
            "UserTaskRemovedEvent" => "user-task",
            "UserTaskStatusChangedEvent" => "user-task",
            _ => throw new ArgumentOutOfRangeException(
                nameof(messageType),
                messageType,
                null
            )
        };

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        LogServiceStoppingGracefully(_logger);
        await base.StopAsync(cancellationToken);
    }
    
    [LoggerMessage(LogLevel.Information, Message = "Сервис процесса аутбокса начал работу (Интервал: {Interval} сек)")]
    private static partial void LogServiceStarted(ILogger logger, int interval);
    
    [LoggerMessage(LogLevel.Information, Message = "Остановка сервиса аутбокса.")]
    private static partial void LogServiceStopping(ILogger logger);
    
    [LoggerMessage(LogLevel.Information, Message = "Сервис аутбокса остановлен")]
    private static partial void LogServiceStopped(ILogger logger);
    
    [LoggerMessage(LogLevel.Error, Message = "Ошибка в ходе процесса аутбокса")]
    private static partial void LogProcessingError(ILogger logger, Exception ex);
    
    [LoggerMessage(LogLevel.Error, Message = "Получено неизвестное сообщение для продьюса: {MessageType}")]
    private static partial void LogUnknownMessageType(ILogger logger, Exception ex, string messageType);
    
    [LoggerMessage(LogLevel.Error, Message = "Ошибка продьюса сообщения {MessageId} (Попытка: {RetryCount})")]
    private static partial void LogPublishError(ILogger logger, Exception ex, long messageId, int retryCount);
    
    [LoggerMessage(LogLevel.Error, Message = "Сообщение {MessageId} превысило максимальное количество попыток отправки ({MaxRetryCount})")]
    private static partial void LogMaxRetryExceeded(ILogger logger, long messageId, int maxRetryCount);
    
    [LoggerMessage(LogLevel.Information, Message = "Корректная остановка процессора аутбокса")]
    private static partial void LogServiceStoppingGracefully(ILogger logger);
}
