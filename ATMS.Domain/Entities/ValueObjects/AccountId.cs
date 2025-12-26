
namespace ATMS.Domain.Entities;

public record AccountId
{
    public AccountId()
    {
    }

    public Guid Value { get; set; }

    public AccountId(Guid value)
    {
        Value = value;
    }

    public static AccountId CreateNew() => new(Guid.NewGuid());
    public static AccountId Create(Guid value) => new(value);
    public static AccountId Empty() => new(Guid.Empty);

}