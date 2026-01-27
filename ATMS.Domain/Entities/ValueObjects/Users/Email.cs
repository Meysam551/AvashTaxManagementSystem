
using ATMS.Domain.Common;
using System.Text.RegularExpressions;

namespace ATMS.Domain.Entities;

public sealed record Email
{
    private static readonly Regex EmailRegex =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public string Value { get; }

    public Email(string value)
    {
        Value = value;
    }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Email is required");

        value = value.Trim();

        if (!EmailRegex.IsMatch(value))
            throw new DomainException("Invalid email format");

        return new Email(value.ToLowerInvariant());
    }

    public static implicit operator string(Email email) => email.Value;
}


