using MediatR;
using MobiGate.Domain.Common;

namespace MobiGate.Application.Auth.Login;

public class LoginHandler(IUserRepository userRepository, IJwtService jwtService) : IRequestHandler<LoginCommand, Result<LoginResult>>
{
    public async Task<Result<LoginResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email.ToLowerInvariant().Trim(), cancellationToken);
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Result<LoginResult>.Failure("Invalid email or password.", 401);

        var (token, expiresAt) = jwtService.GenerateToken(user);
        return Result<LoginResult>.Success(new LoginResult(token, expiresAt));
    }
}
