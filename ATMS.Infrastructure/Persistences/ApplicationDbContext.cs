
using System.Reflection;
using ATMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ATMS.Infrastructure;

public class ApplicationDbContext : BaseDbContext
{
    private readonly IMediator _mediator;

    // Constructors
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    [ActivatorUtilitiesConstructor]
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<DocHead> DocHeads => Set<DocHead>();
    public DbSet<DocItem> DocItems => Set<DocItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("atms");

        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    }
}
