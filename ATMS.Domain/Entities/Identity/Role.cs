using Microsoft.AspNetCore.Identity;

namespace ATMS.Domain.Entities;

public class Role : IdentityRole<Guid>
{
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsSystemRole { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public virtual ICollection<RoleClaim> RoleClaims { get; set; } = new List<RoleClaim>();
}
