using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MobiGate.Application.Auth;
using MobiGate.Infrastructure.Auth;
using MobiGate.Infrastructure.Data;
using MobiGate.Infrastructure.Repositories;

namespace MobiGate.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<MobiGateDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("PostgreSQL")));

        services.Configure<JwtOptions>(config.GetSection(JwtOptions.SectionName).Bind);

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}
