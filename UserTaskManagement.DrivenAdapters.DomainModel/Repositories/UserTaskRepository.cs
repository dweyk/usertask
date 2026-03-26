using Microsoft.EntityFrameworkCore;
using UserTaskManagement.Application.DrivenPorts.DomainModel;
using UserTaskManagement.Domain.Models.UserTask;

namespace UserTaskManagement.DrivenAdapters.DomainModel.Repositories;

/// <inheritdoc/>
internal sealed class UserTaskRepository : IUserTaskRepository
{
    private readonly AppDbContext _dbContext;

    public UserTaskRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public async Task Add(UserTask userTask)
    {
        await _dbContext.Set<UserTask>().AddAsync(userTask);
    }

    /// <inheritdoc/>
    public void Remove(UserTask userTask)
    {
        _dbContext.Set<UserTask>().Remove(userTask);
    }

    /// <inheritdoc/>
    public async Task<UserTask?> GetById(long id, CancellationToken ct)
    {
        return await _dbContext.Set<UserTask>()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<UserTask>> GetAll(CancellationToken ct)
    {
        return await _dbContext.Set<UserTask>()
            .AsNoTracking()
            .ToListAsync(ct);
    }
}