
using System.Reflection;
using ATMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ATMS.Infrastructure;

public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    private readonly IMediator _mediator;

    // Constructors
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<ATMSUser> ATMSUsers => Set<ATMSUser>();

    //public DbSet<DocHead> DocHeads => Set<DocHead>();
    //public DbSet<DocItem> DocItems => Set<DocItem>();
    public DbSet<DocumentCover> DocumentCovers => Set<DocumentCover>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("atms");

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);

        //builder.Entity<DocItem>(entity =>
        //{
        //    entity.HasKey(x => x.Id);
        //    entity.Property(e => e.Id)
        //        .HasConversion(
        //            v => v.Value,           // Convert to string for DB
        //            v => new DocItemId(v)   // Convert from DB
        //        );
        //});
    }
}
