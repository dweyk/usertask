namespace UserTaskManagement.Application.IntegrationEvents;

/// <summary>
/// Базовый класс для всех интеграционных событий
/// </summary>
public abstract class BaseEvent
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="eventType">Тип события</param>
    /// <param name="occurredAt">Время возникновения события</param>
    protected BaseEvent(
        string eventType,
        DateTime occurredAt
    )
    {
        EventType = eventType;
        OccurredAt = occurredAt;
    }
    
    /// <summary>
    /// Тип события
    /// </summary>
    public string EventType { get; }
    
    /// <summary>
    /// Время события
    /// </summary>
    public DateTime OccurredAt { get; }
}
