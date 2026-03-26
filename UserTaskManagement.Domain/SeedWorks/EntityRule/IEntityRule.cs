using FluentResults;
using UserTaskManagement.Domain.SeedWorks.Aggregate.Interfaces;

namespace UserTaskManagement.Domain.SeedWorks.EntityRule;

/// <summary>
/// Правило, применяемое к сущности
/// </summary>
/// <typeparam name="T">Тип сущности</typeparam>
public interface IEntityRule<in T> where T : IEntity
{
    /// <summary>
    /// Применяет правило к сущности
    /// </summary>
    /// <param name="entity">Проверяемая сущность</param>
    Result Apply(T entity);
}
