
namespace ATMS.Domain.Entities;

public sealed record DocItemId
{
    public Guid Value { get; }

    public DocItemId(Guid value) => Value = value;

    public static DocItemId CreateNew() => new(Guid.NewGuid());

    public static DocItemId Create(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
