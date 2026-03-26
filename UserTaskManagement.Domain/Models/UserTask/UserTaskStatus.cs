namespace UserTaskManagement.Domain.Models.UserTask;

/// <summary>
/// Статус задачи пользователя
/// </summary>
public enum UserTaskStatus
{
    /// <summary>
    /// Неизвестно
    /// </summary>
    Unknown = 0,
    
    /// <summary>
    /// Новая
    /// </summary>
    New = 1,
    
    /// <summary>
    /// В работе
    /// </summary>
    InProgress = 2,
    
    /// <summary>
    /// Выполеннная
    /// </summary>
    Completed = 3
}
