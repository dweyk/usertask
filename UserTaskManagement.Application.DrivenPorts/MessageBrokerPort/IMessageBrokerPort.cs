namespace UserTaskManagement.Application.DrivenPorts.MessageBrokerPort;

/// <summary>
/// Порт брокера сообщений
/// </summary>
public interface IMessageBrokerPort
{
    /// <summary>
    /// Отправляет сообщение
    /// </summary>
    /// <param name="topic">Имя топика Kafka</param>
    /// <param name="key">Ключ сообщения (для партиционирования)</param>
    /// <param name="message">Тело сообщения (JSON)</param>
    /// <param name="ct">Токен отмены</param>
    Task PublishAsync(
        string topic,
        string key,
        string message,
        CancellationToken ct
    );
}
