
using Microsoft.AspNetCore.Identity;

namespace ATMS.Infrastructure.Identity;

public class AppRole : IdentityRole<Guid>
{
    public string Description { get; set; } = string.Empty;
}
