namespace UserTaskManagement.Domain.SeedWorks.Aggregate.Interfaces;

/// <summary>
/// Версионируемый объект
/// </summary>
public interface IVersionable
{
    /// <summary>
    /// Версия объекта
    /// </summary>
    uint Version { get; }
}
