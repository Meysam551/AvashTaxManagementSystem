
using ATMS.Domain.Abstracts;
namespace ATMS.Domain.Entities;

public class ATMSUser : AggregateRoot<ATMSUserId>
{
    public string Username { get; private set; }
    public bool IsActive { get; private set; }

    public ATMSUserProfile Profile { get; private set; }

    private ATMSUser() { }

    public ATMSUser(ATMSUserId id, string username, ATMSUserProfile profile)
    {
        Id = id;
        Username = username;
        Profile = profile;
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void UpdateProfile(
        string firstName,
        string lastName,
        Email email)
    {
        Profile = Profile.Update(firstName, lastName, email);
    }
}
