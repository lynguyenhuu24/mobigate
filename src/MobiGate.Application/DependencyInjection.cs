using Microsoft.Extensions.DependencyInjection;
using MobiGate.Application.Common;

namespace MobiGate.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            cfg.AddOpenBehavior(typeof(ResultBehavior<,>));
        });

        return services;
    }
}
