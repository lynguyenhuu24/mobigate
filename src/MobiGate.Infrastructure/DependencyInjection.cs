using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MobiGate.Infrastructure.Data;

namespace MobiGate.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<MobiGateDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("PostgreSQL")));

        return services;
    }
}
