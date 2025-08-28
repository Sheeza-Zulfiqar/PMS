using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PMSApi.Entities;
using PMSApi.Services.Interfaces;

namespace PMSApi.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;

        public AuthService(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor
         )
        {
            _configuration = configuration;
            _httpContext = httpContextAccessor;
         }

        public User GetUser()
        {
            try
            {
                string? authToken = _httpContext.HttpContext?.Request.Headers.Authorization;
                if (authToken == null)
                    return new User { Id = -1, Username = "" };
                JwtSecurityTokenHandler jwtTokenHandler = new();
                JwtSecurityToken jwtSecurityToken = jwtTokenHandler.ReadJwtToken(
                    authToken!.Replace("Bearer ", "")
                );
                return new User
                {
                    Id = int.Parse(jwtSecurityToken.Claims.First(x => x.Type == "Id").Value),
                };
            }
            catch (Exception)
            {
                return new User { Id = -1, Username = "" };
            }
        }

        public string CreateToken(User user)
        {
            var secureKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
            var issuer = _configuration["Jwt:SecurityKey"]!;
            var securityKey = new SymmetricSecurityKey(secureKey);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.Username!),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    }
                ),
                Expires = DateTime.Now.AddHours(2),
                Issuer = issuer,
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
