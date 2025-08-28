using FluentValidation;
using PMSApi.DTOs.UserDtos;
using PMSApi.Filters.ValidationFilters;
using PMSApi.Filters.ValidationFilters.CreateValidators;
using PMSApi.Filters.ValidationFilters.UpdateValidators;

namespace PMSApi.Configurations
{
    public static class AllValidators
    {

        public static IServiceCollection AddAllValidators(this IServiceCollection services)
        {


            services.AddScoped<IValidator<UserRegisterDto>, UserRegisterValidator>();

            services.AddScoped<IValidator<UserUpdateDto>, UserUpdateValidator>();

            services.AddScoped<IValidator<UserLoginDto>, UserLoginValidator>();





            return services;
        }
    }
}
