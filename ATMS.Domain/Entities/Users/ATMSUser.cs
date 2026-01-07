
using ATMS.Domain.Abstracts;
using ATMS.Domain.Common;

namespace ATMS.Domain.Entities;

public class ATMSUser : AggregateRoot<ATMSUserId>
{
    public string Username { get; private set; }
    public bool IsActive { get; private set; }
    public ATMSUserProfile Profile { get; private set; }

    private ATMSUser() { } // For EF

    public ATMSUser(
        ATMSUserId id,
        string username,
        ATMSUserProfile profile)
    {
        Id = id;
        Username = username;
        Profile = profile;
        IsActive = true;
    }

    // -------------------------
    // Factory
    // -------------------------
    public static ATMSUser Create(
        ATMSUserId id,
        string username,
        ATMSUserProfile profile)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new DomainException("Username is required");

        return new ATMSUser(
            id,
            username.Trim(),
            profile);
    }

    // -------------------------
    // Behavior
    // -------------------------
    public void Deactivate()
    {
        if (!IsActive)
            return; 

        IsActive = false;
    }

    public void UpdateProfile(
        string firstName,
        string lastName,
        Email email)
    {
        Profile = Profile.Update(
            firstName,
            lastName,
            email);
    }
}

