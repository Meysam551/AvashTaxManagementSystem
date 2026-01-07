using ATMS.Domain.Entities;
using ErrorOr;
using MediatR;

namespace ATMS.ApplicationService;

public sealed record AssignRoleCommand(
    ATMSUserId UserId,
    string RoleName
) : IRequest<ErrorOr<Success>>;

