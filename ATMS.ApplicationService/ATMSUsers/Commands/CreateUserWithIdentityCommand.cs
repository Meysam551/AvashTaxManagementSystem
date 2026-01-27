
using ATMS.Domain.Contracts;
using ATMS.Domain.Entities;
using ErrorOr;
using MediatR;

namespace ATMS.ApplicationService;

public sealed record CreateUserWithIdentityCommand(
    string Username,
    string FirstName,
    string LastName,
    string Email,
    string Password
) : IRequest<ErrorOr<Guid>>;

public sealed class CreateUserWithIdentityHandler
    : IRequestHandler<CreateUserWithIdentityCommand, ErrorOr<Guid>>
{
    private readonly IUserRepository _userRepo;
    private readonly IIdentityRepository _identityRepo;

    public CreateUserWithIdentityHandler(
        IUserRepository userRepo,
        IIdentityRepository identityRepo)
    {
        _userRepo = userRepo;
        _identityRepo = identityRepo;
    }

    public async Task<ErrorOr<Guid>> Handle(
        CreateUserWithIdentityCommand cmd,
        CancellationToken ct)
    {
        var identityUserId =
            await _identityRepo.CreateAsync(
                cmd.Username,
                cmd.Email,
                cmd.Password,
                ct);

        try
        {
            var profile = ATMSUserProfile.Create(
                cmd.FirstName,
                cmd.LastName,
                Email.Create(cmd.Password)
            );

            var domainUser = ATMSUser.Create(
                identityUserId,
                cmd.Username,
                profile);

            await _userRepo.AddAsync(domainUser, ct);

            return domainUser.Id.Value;
        }
        catch
        {
            await _identityRepo.DeleteAsync(identityUserId, ct);
            throw;
        }
    }
}



