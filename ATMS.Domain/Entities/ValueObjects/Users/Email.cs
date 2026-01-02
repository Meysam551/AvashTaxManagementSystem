
using ATMS.Domain.Common;
using ATMS.Shared;

namespace ATMS.Domain.Entities;

public class Email : ValueObject
{
    public string Value { get; }

    private Email() { }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Email is required");

        // ساده ولی کافی
        if (!value.Contains("@"))
            throw new DomainException("Invalid email");

        Value = value.ToLowerInvariant();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(Email email) => email.Value;
}

