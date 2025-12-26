
using ATMS.Domain.Common;

namespace ATMS.Domain.Entities;

public sealed class DocumentLine
{
    public AccountId AccountId { get; }
    public SubAccountId? SubAccountId { get; }
    public CostCenterId? CostCenterId { get; }

    public decimal Debit { get; }
    public decimal Credit { get; }

    public string Description { get; }

    internal DocumentLine(
        AccountId accountId,
        decimal debit,
        decimal credit,
        string description,
        SubAccountId? subAccountId = null,
        CostCenterId? costCenterId = null)
    {
        if (debit > 0 && credit > 0)
            throw new DomainException("Line cannot have both debit and credit");

        if (debit <= 0 && credit <= 0)
            throw new DomainException("Either debit or credit must be greater than zero");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Line description is required");

        AccountId = accountId;
        Debit = debit;
        Credit = credit;
        Description = description;

        SubAccountId = subAccountId;
        CostCenterId = costCenterId;
    }
}
