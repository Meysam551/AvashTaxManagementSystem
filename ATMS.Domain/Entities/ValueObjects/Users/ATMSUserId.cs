
namespace ATMS.Domain.Entities;

public readonly record struct ATMSUserId(Guid Value)
{
    public static ATMSUserId New() => new(Guid.NewGuid());
}


