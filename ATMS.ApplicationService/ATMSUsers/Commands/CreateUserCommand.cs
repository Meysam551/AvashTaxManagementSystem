
using ATMS.Domain.Contracts;
using ATMS.Domain.Entities;
using ErrorOr;
using MediatR;

namespace ATMS.ApplicationService;

public sealed record CreateUserCommand(
    string Username,
    string Password,
    string FirstName,
    string LastName,
    string Email
) : IRequest<ErrorOr<ATMSUserId>>;

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ErrorOr<ATMSUserId>>
{
    private readonly IIdentityService _identityRepository;
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IIdentityService identityRepository, IUserRepository userRepository)
    {
        this._identityRepository = identityRepository;
        this._userRepository = userRepository;
    }

    public async Task<ErrorOr<ATMSUserId>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var userId = await _identityRepository.CreateUserAsync(
            request.Username,
            request.Password,
            request.Email,
            cancellationToken);

        var profile = new ATMSUserProfile(
            request.FirstName,
            request.LastName,
            new Email(request.Email));

        var user = new ATMSUser(userId, request.Username, profile);

        await _userRepository.AddAsync(user, cancellationToken);

        return userId;
    }
}