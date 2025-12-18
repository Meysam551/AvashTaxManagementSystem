
using ATMS.Domain.Contracts;
using ATMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ATMS.Infrastructure;

public class DocHeadRepository : IDocHeadRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContext;

    public DocHeadRepository(IDbContextFactory<ApplicationDbContext> dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<DocHeadId> AddDocHeadAsync(DocHead model, CancellationToken cancellationToken)
    {
        await using var context = await _dbContext.CreateDbContextAsync();

        context.Add(model);

        await context.SaveChangesAsync(cancellationToken);

        return model.Id;
    }
}
