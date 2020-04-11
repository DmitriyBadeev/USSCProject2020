using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using USSC.Infrastructure;
using USSC.Infrastructure.Services;
using USSC.Services.PermissionServices;
using USSC.Services.UserServices;
using USSC.Services.UserServices.Interfaces;

namespace USSC.Services
{
    public static class ServicesCollectionExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IApplicationDataService, ApplicationDataService>();
            services.AddScoped<ISeedData, SeedData>();

            return services;
        }

        public static IServiceCollection AddAccountServices(this IServiceCollection services)
        {
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IUserDataService, UserDataService>();
            services.AddScoped<IAccessManager, AccessManager>();

            return services;
        }
    }
}
