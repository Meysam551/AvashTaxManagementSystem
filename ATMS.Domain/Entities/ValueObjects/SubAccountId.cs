
namespace ATMS.Domain.Entities;

public record SubAccountId
{
    public SubAccountId()
    {
    }

    public Guid Value { get; set; }

    public SubAccountId(Guid value)
    {
        Value = value;
    }

    public static SubAccountId CreateNew()  => new(Guid.NewGuid());
    public static SubAccountId Create(Guid value)  => new(value);
    public static SubAccountId Empty() => new(Guid.Empty);
}