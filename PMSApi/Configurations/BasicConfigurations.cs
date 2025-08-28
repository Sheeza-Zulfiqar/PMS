using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PMSApi.Data;
using PMSApi.Middleware;

namespace PMSApi.Configurations
{
    public static class BasicConfigurations
    {
        private const string CorsPolicyName = "CorsPolicy";

        public static IServiceCollection ConfigureServices(
            this IServiceCollection services,
            ConfigurationManager configuration
        )
        {
            services.AddCors(options =>
            {
                options.AddPolicy(
                    name: CorsPolicyName,
                    policy =>
                    {
                        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().Build();
                    }
                );
            });

            //Authentication
            services
                .AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)
                        ),
                        ValidateLifetime = true
                    };
                });

            services.AddAuthorization(o =>
                o.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build()
            );

            services.AddHttpContextAccessor();

            var sqlConnectionString = configuration.GetConnectionString("SQLDbConnection");
 
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseSqlServer(sqlConnectionString, b => b.MigrationsAssembly("PMSApi"));
            });

           
        
            return services;
        }

        public static WebApplication ConfigureApp(this WebApplication webApplication)
        {
            webApplication.UseHttpsRedirection();

            webApplication.UseCors(CorsPolicyName);

            webApplication.UseAuthentication();
            webApplication.UseAuthorization();

            webApplication.UseMiddleware<ResponseHeadersMiddleware>();

            return webApplication;
        }
    }

}
