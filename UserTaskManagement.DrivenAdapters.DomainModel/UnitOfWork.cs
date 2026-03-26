using System.Transactions;
using UserTaskManagement.Application;

namespace UserTaskManagement.DrivenAdapters.DomainModel;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public TransactionScope BeginTransaction()
    {
       return new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted},
            TransactionScopeAsyncFlowOption.Enabled);
    }

    public async Task SaveChanges(CancellationToken ct)
    {
        await _dbContext.SaveChangesAsync(ct);
    }

    /// <inheritdoc/>
    public async Task Commit(TransactionScope scope, CancellationToken ct)
    {
        await _dbContext.SaveChangesAsync(ct);
        scope.Complete();
    }
}
