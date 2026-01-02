
namespace ATMS.Domain.Entities;

public sealed record DocHeadId
{
    public Guid Value { get; }

    public DocHeadId(Guid value) => Value = value;

    public static DocHeadId CreateNew() => new(Guid.NewGuid());

    public static DocHeadId Create(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
