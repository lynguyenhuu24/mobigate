using MediatR;
using MobiGate.Domain.Common;
using MobiGate.Domain.Entities;
using MobiGate.Domain.Enums;

namespace MobiGate.Application.Auth.Register;

public class RegisterHandler(IUserRepository userRepository) : IRequestHandler<RegisterCommand, Result<RegisterResult>>
{
    public async Task<Result<RegisterResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existing = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existing is not null)
            return Result<RegisterResult>.Failure("Email is already registered.", 409);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email.ToLowerInvariant().Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = UserRole.Member,
            CreatedAt = DateTime.UtcNow
        };

        await userRepository.AddAsync(user, cancellationToken);

        return Result<RegisterResult>.Success(new RegisterResult(user.Id));
    }
}
