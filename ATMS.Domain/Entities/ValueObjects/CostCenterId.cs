
namespace ATMS.Domain.Entities;

public record CostCenterId
{
    public CostCenterId()
    {
    }

    public Guid Value { get; set; }

    public CostCenterId(Guid value)
    {
        Value = value;
    }

    public static CostCenterId CreateNew() => new(Guid.NewGuid());
    public static CostCenterId Create(Guid value) => new(value);
    public static CostCenterId Empty() => new(Guid.Empty);
}