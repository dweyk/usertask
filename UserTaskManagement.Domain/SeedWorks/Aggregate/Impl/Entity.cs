using UserTaskManagement.Domain.SeedWorks.Aggregate.Interfaces;

namespace UserTaskManagement.Domain.SeedWorks.Aggregate.Impl;

public abstract class Entity : IEntity
{
    protected Entity(long id)
    {
        Id = id;
    }
    
    public long Id { get; private set; }

    public bool IsTransient => Id == 0L;
}
