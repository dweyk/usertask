using UserTaskManagement.Domain.Models.UserTask;

namespace UserTaskManagement.Application.DrivenPorts.DomainModel;

/// <summary>
/// Репозиторий пользовательских задач
/// </summary>
public interface IUserTaskRepository
{
    /// <summary>
    /// Добавляет новый задачу
    /// </summary>
    Task Add(UserTask userTask);
    
    /// <summary>
    /// Удаляет задачу
    /// </summary>
    void Remove(UserTask userTask);
    
    /// <summary>
    /// Получает задачу по ID
    /// </summary>
    Task<UserTask?> GetById(long id, CancellationToken ct);
    
    /// <summary>
    /// Получает список всех задач
    /// </summary>
    Task<IReadOnlyList<UserTask>> GetAll(CancellationToken ct);
}
