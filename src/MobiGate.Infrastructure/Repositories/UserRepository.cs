using Microsoft.EntityFrameworkCore;
using MobiGate.Application.Auth;
using MobiGate.Domain.Entities;
using MobiGate.Infrastructure.Data;

namespace MobiGate.Infrastructure.Repositories;

public class UserRepository(MobiGateDbContext db) : IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        return await db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
    }

    public async Task AddAsync(User user, CancellationToken ct = default)
    {
        db.Users.Add(user);
        await db.SaveChangesAsync(ct);
    }
}
