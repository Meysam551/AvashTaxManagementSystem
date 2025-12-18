
namespace ATMS.Domain.Entities;

public record DocItemId
{
    public DocItemId()
    {
    }

    public Guid Value { get; set; }

    public DocItemId(Guid value)
    {
        Value = value;
    }

    public static DocItemId CreateNew() => new(Guid.NewGuid());
    public static DocItemId Create(Guid value) => new(value);
    public static DocItemId Empty() => new(Guid.Empty);
}
