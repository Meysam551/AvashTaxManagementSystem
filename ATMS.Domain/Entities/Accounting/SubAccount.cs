
using ATMS.Domain.Common;

namespace ATMS.Domain.Entities;

public sealed class SubAccount
{
    public SubAccountId Id { get; init; }
    public AccountId AccountId { get; init; }
    public string Title { get; init; }

    private SubAccount() { }

    public static SubAccount Create(AccountId accountId, string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("SubAccount title is required");

        return new SubAccount
        {
            Id = SubAccountId.CreateNew(),
            AccountId = accountId,
            Title = title
        };
    }
}

