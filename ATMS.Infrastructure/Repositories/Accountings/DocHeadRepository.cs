
using ATMS.Domain.Contracts;
using ATMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ATMS.Infrastructure;

public class DocHeadRepository : IDocHeadRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContext;
    private readonly ILogger<DocHeadRepository> _logger;

    public DocHeadRepository(IDbContextFactory<ApplicationDbContext> dbContext, ILogger<DocHeadRepository> logger)
    {
        this._dbContext = dbContext;
        this._logger = logger;
    }

    public async Task<DocHeadId> AddDocHeadAsync(DocHead model, CancellationToken cancellationToken)
    {
        try
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            context.Add(model);

            await context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "DocHead {@DocHeadId} saved successfully",
                model.Id);

            return model.Id;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning(
                "AddDocHead operation was cancelled for DocHead {DocHeadId}",
                model.Id);

            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(
                ex,
                "Database error while adding DocHead {DocHeadId}",
                model.Id);

            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while adding DocHead {DocHeadId}",
                model.Id);

            throw;
        }
    }

    public async Task<IReadOnlyList<DocHead>> GetListAsync(CancellationToken cancellationToken)
    {
        try
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            var list = await context.DocHeads.ToListAsync();
            _logger.LogInformation(
                "Fetched {Count} DocHead records",
                list.Count);
            return list;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("GetListAsync operation was cancelled");
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(
                ex,
                "Database error while fetching DocHead list");

            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while fetching DocHead list");

            throw;
        }
    }
}
