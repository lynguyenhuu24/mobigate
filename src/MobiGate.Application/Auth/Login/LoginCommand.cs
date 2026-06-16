using MediatR;
using MobiGate.Domain.Common;

namespace MobiGate.Application.Auth.Login;

public record LoginCommand(string Email, string Password) : IRequest<Result<LoginResult>>;
