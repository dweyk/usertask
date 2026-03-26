using System.Transactions;

namespace UserTaskManagement.Application;

public interface IUnitOfWork
{
    /// <summary>
    /// Начать транзакцию
    /// </summary>
    public TransactionScope BeginTransaction();

    /// <summary>
    /// Сохранить изменения
    /// </summary>
    Task SaveChanges(CancellationToken ct);
    
    /// <summary>
    /// Фиксирует транзакцию
    /// </summary>
    Task Commit(TransactionScope scope, CancellationToken ct);
}
