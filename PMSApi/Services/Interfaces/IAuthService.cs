using PMSApi.Entities;

namespace PMSApi.Services.Interfaces
{
    public interface IAuthService
    {
        string CreateToken(User user);
        User GetUser();
    }
}
