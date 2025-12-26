
using ATMS.Domain.Abstracts;
using ATMS.Domain.Common;

namespace ATMS.Domain.Entities;

public sealed class CostCenter : AggregateRoot<CostCenterId>
{
    public string Code { get; private set; }
    public string Title { get; private set; }
    public bool IsActive { get; private set; }

    private CostCenter() { }

    public static CostCenter Create(string code, string title)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new DomainException("کد مرکز هزینه الزامی میباشد");

        return new CostCenter
        {
            Id = CostCenterId.CreateNew(),
            Code = code,
            Title = title,
            IsActive = true
        };
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
