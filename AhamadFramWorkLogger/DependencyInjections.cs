using AhamadFramWorkLogger.Configes;
using AhamadFramWorkLogger.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;
namespace AhamadFramWorkLogger
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddseriLogConfigFramWork(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<ISerilogService, SerilogService>();
            SerilogConfigs.UseSqlServer(configuration);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }

    }
}
