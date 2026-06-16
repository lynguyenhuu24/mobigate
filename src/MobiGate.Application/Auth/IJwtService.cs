using MobiGate.Domain.Entities;

namespace MobiGate.Application.Auth;

public interface IJwtService
{
    (string token, DateTime expiresAt) GenerateToken(User user);
}
