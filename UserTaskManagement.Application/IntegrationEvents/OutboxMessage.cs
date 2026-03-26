namespace UserTaskManagement.Application.IntegrationEvents;

/// <summary>
/// Outbox
/// </summary>
public class OutboxMessage
{
    // идентификатор события
    public long Id { get;  set; }

    /// <summary>
    /// Тип события
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Данные события в формате JSON
    /// </summary>
    public string Data { get; set; }

    /// <summary>
    /// Время создания записи
    /// </summary>
    public DateTime OccurredOn { get; set; }

    /// <summary>
    /// Время отправки в Kafka
    /// </summary>
    public DateTime? ProcessedAt { get; set; }

    /// <summary>
    /// Счётчик попыток отправки
    /// </summary>
    public int RetryCount { get; set; }

    // // конструктор без параметров для EF Core
    // private OutboxMessage()
    // {
    // }

    public OutboxMessage(
        string type,
        string data,
        DateTime occurredOn
    )
    {
        Type = type;
        Data = data;
        OccurredOn = occurredOn;
        ProcessedAt = null;
        RetryCount = 0;
    }
}
