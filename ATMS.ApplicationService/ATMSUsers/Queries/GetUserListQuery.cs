

using ATMS.Domain.Contracts;
using ErrorOr;
using MediatR;

namespace ATMS.ApplicationService;

public sealed record GetUserListQuery() : IRequest<ErrorOr<IEnumerable<ATMSUserDto>>>;

public sealed class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, ErrorOr<IEnumerable<ATMSUserDto>>>
{
    private readonly IUserRepository _userRepository;

    public GetUserListQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<IEnumerable<ATMSUserDto>>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetListAsync(cancellationToken);

        var dtos = users.Select(u => new ATMSUserDto
        {
            Id = u.Id.Value,
            Username = u.Username,
            Email = u.Profile.Email.Value,
            FirstName = u.Profile.FirstName,
            LastName = u.Profile.LastName
        }).ToList();

        return dtos;
    }
}

