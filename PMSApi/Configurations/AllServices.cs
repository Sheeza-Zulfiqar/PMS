using Microsoft.Extensions.Logging;
using PMSApi.Services.Implementations;
using PMSApi.Services.Interfaces;

namespace PMSApi.Configurations
{
    public static class AllServices
    {
        public static IServiceCollection AddAllServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
       
            return services;
        }
    }

}
