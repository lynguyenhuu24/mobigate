using MobiGate.Domain.Entities;

namespace MobiGate.Application.Auth;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task AddAsync(User user, CancellationToken ct = default);
}
