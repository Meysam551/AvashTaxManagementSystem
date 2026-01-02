
using ATMS.Domain.Entities;

namespace ATMS.Domain.Contracts;

public interface IIdentityRepository
{
    Task<ATMSUserId> CreateUserAsync(string username, string password, string email);
}