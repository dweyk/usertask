namespace UserTaskManagement.DrivenAdapters.MessageBroker;

/// <summary>
/// Настройки подключения к Kafka
/// </summary>
public class KafkaOptions
{
    /// <summary>
    /// Адреса брокеров (через запятую)
    /// </summary>
    public string BootstrapServers { get; set; } = "kafka:29092";
    
    /// <summary>
    /// Топик по умолчанию (если не указан в сообщении)
    /// </summary>
    public string DefaultTopic { get; set; } = "user-task";
}
