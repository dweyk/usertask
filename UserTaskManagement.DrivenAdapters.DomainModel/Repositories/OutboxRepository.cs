using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using UserTaskManagement.Application.DrivenPorts.DomainModel;
using UserTaskManagement.Application.IntegrationEvents;
using UserTaskManagement.Domain.SeedWorks;

namespace UserTaskManagement.DrivenAdapters.DomainModel.Repositories;

/// <inheritdoc/>
public class OutboxRepository : IOutboxRepository
{
    private readonly AppDbContext _dbContext;
    private readonly JsonSerializerOptions _jsonOptions;

    public OutboxRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    /// <inheritdoc/>
    public async Task Add<TEvent>(
        TEvent @event,
        CancellationToken ct
    )
        where TEvent : class
    {
        var message = new OutboxMessage(
            type: typeof(TEvent).Name,
            data: JsonSerializer.Serialize(
                @event,
                _jsonOptions
            ),
            occurredOn: SysTime.UtcNow()
        );

        await _dbContext.OutboxMessages.AddAsync(
            message,
            ct
        );
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<OutboxMessage>> GetUnprocessed(
        int limit,
        CancellationToken ct
    )
    {
        return await _dbContext.OutboxMessages
            .AsNoTracking()
            .Where(m => m.ProcessedAt == null)
            .OrderBy(m => m.OccurredOn)
            .Take(limit)
            .ToListAsync(ct);
    }

    /// <inheritdoc/>
    public async Task MarkAsProcessed(
        long messageId,
        CancellationToken ct = default
    )
    {
        var message = await _dbContext.OutboxMessages
            .FindAsync([messageId], ct);

        if (message != null)
        {
            message.ProcessedAt = SysTime.UtcNow();
        }
    }

    /// <inheritdoc/>
    public async Task IncrementRetryCount(
        long messageId,
        CancellationToken ct = default
    )
    {
        var message = await _dbContext.OutboxMessages
            .FindAsync([messageId], ct);

        if (message != null)
        {
            message.RetryCount += 1;
        }
    }
}
