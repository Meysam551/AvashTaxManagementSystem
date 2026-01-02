
using ATMS.Domain.Common;
using ATMS.Shared;

namespace ATMS.Domain.Entities;

public class ATMSUserProfile : ValueObject
{
    public string FirstName { get; }
    public string LastName { get; }
    public Email Email { get; }

    private ATMSUserProfile() { }

    public ATMSUserProfile(
        string firstName,
        string lastName,
        Email email)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name is required");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name is required");

        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public ATMSUserProfile Update(
        string firstName,
        string lastName,
        Email email)
    {
        return new ATMSUserProfile(firstName, lastName, email);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
        yield return Email;
    }
}
