using UserTaskManagement.Application.IntegrationEvents;

namespace UserTaskManagement.Application.DrivenPorts.DomainModel;

/// <summary>
/// Репозиторий аутбокса
/// </summary>
public interface IOutboxRepository
{
    /// <summary>
    /// Добавляет новое событие в аутбокс
    /// </summary>
    /// <typeparam name="TEvent">Тип интеграционного события</typeparam>
    /// <param name="event">Событие для публикации</param>
    Task Add<TEvent>(TEvent @event, CancellationToken ct)
        where TEvent : class;
    
    /// <summary>
    /// Получает список необработанных событий для отправки
    /// </summary>
    /// <param name="limit">Максимальное количество записей</param>
    Task<IReadOnlyList<OutboxMessage>> GetUnprocessed(int limit, CancellationToken ct);
    
    /// <summary>
    /// Помечает событие как успешно отправленное
    /// </summary>
    /// <param name="messageId">ID сообщения</param>
    Task MarkAsProcessed(long messageId, CancellationToken ct);
    
    /// <summary>
    /// Увеличивает счётчик попыток отправки
    /// </summary>
    /// <param name="messageId">ID сообщения</param>
    Task IncrementRetryCount(long messageId, CancellationToken ct);
}
