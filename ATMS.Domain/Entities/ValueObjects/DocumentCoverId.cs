
namespace ATMS.Domain.Entities;

public record DocumentCoverId
{
    public DocumentCoverId()
    {
    }

    public Guid Value { get; set; }

    public DocumentCoverId(Guid value)
    {
        Value = value;
    }

    public static DocumentCoverId CreateNew() => new(Guid.NewGuid());
    public static DocumentCoverId Create(Guid value) => new(value);
    public static DocumentCoverId Empty() => new(Guid.Empty);
}