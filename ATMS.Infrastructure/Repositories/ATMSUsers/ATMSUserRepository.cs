
using ATMS.Domain.Contracts;
using ATMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ATMS.Infrastructure;

public class ATMSUserRepository : IUserRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;

    public ATMSUserRepository(IDbContextFactory<ApplicationDbContext> context)
    {
        _context = context;
    }

    public async Task<ATMSUser?> GetByIdAsync(ATMSUserId userId, CancellationToken ct)
    {
        await using var context = await _context.CreateDbContextAsync(ct);

        return await context.ATMSUsers
            .FirstOrDefaultAsync(x => x.Id == userId, ct);
    }

    public async Task<ATMSUser?> GetByUsernameAsync(string username, CancellationToken ct)
    {
        await using var context = await _context.CreateDbContextAsync(ct);

        return await context.ATMSUsers
            .FirstOrDefaultAsync(x => x.Username == username, ct);
    }

    public async Task AddAsync(ATMSUser user, CancellationToken ct)
    {
        await using var context = await _context.CreateDbContextAsync(ct);
        await context.ATMSUsers.AddAsync(user, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(ATMSUser user, CancellationToken ct)
    {
        await using var context = await _context.CreateDbContextAsync(ct);
        context.ATMSUsers.Update(user);
        await context.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(ATMSUserId userId, CancellationToken ct)
    {
        await using var context = await _context.CreateDbContextAsync(ct);

        return await context.ATMSUsers.AnyAsync(x => x.Id == userId, ct);
    }
}
