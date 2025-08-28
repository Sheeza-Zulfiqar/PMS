using PMSApi.Entities;
using PMSApi.Interfaces.BaseRepos;

namespace PMSApi.Interfaces
{
    public interface IUserRepo : IBaseRepo<User>
    {
         Task<User?> LoginUserAsync(User User);
        Task<bool> UserExistsAsync(string username);
          Task<User?> GetUserByMobileNumber(string mobile);
        Task<bool> InUseUsername(User user);
     }

}
