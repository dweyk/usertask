using UserTaskManagement.Domain.SeedWorks.Aggregate.Interfaces;

namespace UserTaskManagement.Domain.SeedWorks.Aggregate.Impl;

/// <summary>
/// Сущность с версией
/// </summary>
public abstract class VersionableEntity : Entity, IVersionable
{
    public VersionableEntity(long id, uint version) : base(id)
    {
        Version = version;
    }

    public uint Version { get; private set; }
}
