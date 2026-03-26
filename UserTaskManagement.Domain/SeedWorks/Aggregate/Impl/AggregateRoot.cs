using UserTaskManagement.Domain.SeedWorks.Aggregate.Interfaces;

namespace UserTaskManagement.Domain.SeedWorks.Aggregate.Impl;

/// <summary>
/// Корень агрегата
/// </summary>
public abstract class AggregateRoot : VersionableEntity, IAggregateRoot
{
    protected AggregateRoot(long id, uint version) : base(id, version) {}
}
