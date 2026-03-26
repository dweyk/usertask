using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserTaskManagement.Application.DrivenPorts.MessageBrokerPort;

namespace UserTaskManagement.DrivenAdapters.MessageBroker;

/// <summary>
/// Адаптер брокера сообщений
/// </summary>
public sealed partial class KafkaMessageBrokerAdapter : IMessageBrokerPort, IDisposable
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaMessageBrokerAdapter> _logger;
    private readonly KafkaOptions _options;

    public KafkaMessageBrokerAdapter(
        IOptions<KafkaOptions> options,
        ILogger<KafkaMessageBrokerAdapter> logger
    )
    {
        _logger = logger;
        _options = options.Value;
        
        var config = BuildProducerConfig();
        _producer = BuildProducer(config);
    }

    /// <inheritdoc/>
    public async Task PublishAsync(
        string topic,
        string key,
        string message,
        CancellationToken ct = default
    )
    {
        try
        {
            await _producer.ProduceAsync(
                topic,
                new Message<string, string> { Key = key, Value = message },
                ct
            );
        }
        catch (ProduceException<string, string> ex)
        {
            LogProduceError(_logger, ex, topic, ex.Error.Reason);
            throw;
        }
        catch (Exception ex)
        {
            LogUnknownError(_logger, ex, topic);
            throw;
        }
    }

    public void Dispose()
    {
        _producer?.Flush(TimeSpan.FromSeconds(10));
        _producer?.Dispose();
    }
    
    private IProducer<string, string> BuildProducer(ProducerConfig config)
    {
        var producer = new ProducerBuilder<string, string>(config)
            .SetKeySerializer(Serializers.Utf8)
            .SetValueSerializer(Serializers.Utf8)
            .SetErrorHandler((_, e) => LogProducerError(_logger, e.Reason))
            .SetLogHandler((_, m) => LogProducerDebug(_logger, m.Level, m.Message))
            .Build();

        LogKafkaInitialized(_logger, _options.BootstrapServers);

        return producer;
    }

    private ProducerConfig BuildProducerConfig()
    {
        return new ProducerConfig
        {
            BootstrapServers = _options.BootstrapServers,
            EnableIdempotence = true,
            Acks = Acks.All,
            MessageSendMaxRetries = 5,
            RetryBackoffMs = 100,
            CompressionType = CompressionType.Zstd,
            LingerMs = 5,
            BatchSize = 100000,
        };
    }

    [LoggerMessage(LogLevel.Error, Message = "Ошибка отправки сообщения в {Topic}: {Error}")]
    
    private static partial void LogProduceError(ILogger logger, Exception ex, string topic, string error);

    [LoggerMessage(LogLevel.Error, Message = "Неизвестная ошибка при отправке сообщения в {Topic}")]
    
    private static partial void LogUnknownError(ILogger logger, Exception ex, string topic);

    [LoggerMessage(LogLevel.Error, Message = "Ошибка продьюсера kafka: {Error}")]
    
    private static partial void LogProducerError(ILogger logger, string error);

    [LoggerMessage(LogLevel.Debug, Message = "Kafka: {Level} {Message}")]
    
    private static partial void LogProducerDebug(ILogger logger, SyslogLevel level, string message);

    [LoggerMessage(LogLevel.Information, Message = "Kafka инициализирована {Servers}")]
    
    private static partial void LogKafkaInitialized(ILogger logger, string servers);
}