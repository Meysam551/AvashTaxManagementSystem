
namespace ATMS.Domain.Entities;

public sealed record DocumentCoverId
{
    public Guid Value { get; }

    public DocumentCoverId(Guid value) => Value = value;

    public static DocumentCoverId CreateNew() => new(Guid.NewGuid());

    public static DocumentCoverId Of(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}