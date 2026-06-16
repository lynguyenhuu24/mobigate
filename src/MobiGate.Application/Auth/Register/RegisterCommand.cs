using MediatR;
using MobiGate.Domain.Common;

namespace MobiGate.Application.Auth.Register;

public record RegisterCommand(string Email, string Password) : IRequest<Result<RegisterResult>>;
