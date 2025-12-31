using ATMS.Shared;
using Microsoft.AspNetCore.Identity;

namespace ATMS.Domain.Entities;

public class RoleClaim : IdentityRoleClaim<Guid>
{
    // Extended properties
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public Guid? ModifiedBy { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Role Role { get; set; } = null!;

    // Domain methods
    public void Deactivate()
    {
        IsActive = false;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateClaim(string claimType, string claimValue, string? description = null)
    {
        ClaimType = claimType;
        ClaimValue = claimValue;
        Description = description;
        ModifiedAt = DateTime.UtcNow;
    }
}

// Additional value objects for claims
public record ClaimTypeValue(string Type, string Value, string? Description = null);

public class Permission : ValueObject
{
    public string Area { get; }
    public string Action { get; }
    public string Resource { get; }
    public string Description { get; }

    public Permission(string area, string action, string resource, string description)
    {
        Area = area;
        Action = action;
        Resource = resource;
        Description = description;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Area;
        yield return Action;
        yield return Resource;
    }

    public override string ToString() => $"{Area}.{Action}.{Resource}";

    public static Permission Create(string area, string action, string resource, string description)
        => new(area, action, resource, description);
}
