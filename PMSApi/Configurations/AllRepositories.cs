using Microsoft.Extensions.Logging;
using PMSApi.Interfaces;
using PMSApi.Repositories;

namespace PMSApi.Configurations
{
    public static class AllRepositories
    {
        public static IServiceCollection AddAllRepositories(this IServiceCollection services)
        {
         
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IProjectRepo, ProjectRepo>();
            services.AddScoped<IProjectTaskRepo, ProjectTaskRepo>();



            return services;
        }
    }

}
