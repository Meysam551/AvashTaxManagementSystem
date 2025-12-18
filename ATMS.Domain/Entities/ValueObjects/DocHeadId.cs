
namespace ATMS.Domain.Entities;

public record DocHeadId
{
    public DocHeadId()
    {
    }

    public Guid Value { get; set; }

    public DocHeadId(Guid value)
    {
        Value = value;
    }

    public static DocHeadId CreateNew() => new(Guid.NewGuid());
    public static DocHeadId Create(Guid value) => new(value);
    public static DocHeadId Empty() => new(Guid.Empty);
}
