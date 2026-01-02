
using Microsoft.AspNetCore.Identity;

namespace ATMS.Infrastructure;

public class ApplicationUser : IdentityUser<Guid>
{
    // فقط چیزهایی که Identity واقعاً لازم دارد
    // نه اطلاعات بیزینسی دامنه

    public bool IsActive { get; set; } = true;
}
