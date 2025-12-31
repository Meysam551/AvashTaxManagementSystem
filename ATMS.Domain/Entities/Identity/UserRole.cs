using Microsoft.AspNetCore.Identity;

namespace ATMS.Domain.Entities;

public class UserRole : IdentityUserRole<Guid>
{
    public virtual User User { get; set; } = null!;
    public virtual Role Role { get; set; } = null!;
    public DateTime AssignedAt { get; set; }
    public Guid? AssignedBy { get; set; }
}
