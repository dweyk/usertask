namespace UserTaskManagement.Domain.SeedWorks.Aggregate.Interfaces;

/// <summary>
/// Сущность
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Идентификатор сущности
    /// </summary>
    long Id { get; }
    
    /// <summary>
    /// Является ли сущность только что созданной
    /// </summary>
    bool IsTransient { get; }
}
