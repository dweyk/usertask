namespace UserTaskManagement.DrivenAdapters.OutboxProcessor;

/// <summary>
/// Настройки процессора Outbox
/// </summary>
public class OutboxProcessorOptions
{
    /// <summary>
    /// Как часто проверять новые сообщения
    /// </summary>
    public int PollIntervalSeconds { get; set; } = 10;
    
    /// <summary>
    /// Максимальное количество сообщений за один цикл
    /// </summary>
    public int BatchSize { get; set; } = 100;
    
    /// <summary>
    /// Максимальное количество попыток отправки
    /// </summary>
    public int MaxRetryCount { get; set; } = 5;
}
