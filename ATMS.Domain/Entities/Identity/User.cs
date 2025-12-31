using ATMS.Shared;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ATMS.Domain.Entities;

public class User : IdentityUser<Guid>
{
    // Common properties
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    public DatabaseType DatabaseType { get; set; }
    public string? DatabaseConnectionId { get; set; }

    // Navigation properties
    public virtual ICollection<UserClaim> Claims { get; set; } = new List<UserClaim>();
    public virtual ICollection<UserLogin> Logins { get; set; } = new List<UserLogin>();
    public virtual ICollection<UserToken> Tokens { get; set; } = new List<UserToken>();
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    // Domain methods
    public string GetFullName() => $"{FirstName} {LastName}";

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateProfile(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        UpdatedAt = DateTime.UtcNow;
    }
}
