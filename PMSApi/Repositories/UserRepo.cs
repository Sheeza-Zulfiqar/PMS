using Microsoft.EntityFrameworkCore;
using PMSApi.Data;
using PMSApi.Entities;
using PMSApi.Interfaces;
using PMSApi.Repositories.BaseRepos;

namespace PMSApi.Repositories
{
    public class UserRepo(ApplicationDbContext context) : BaseRepo<User>(context), IUserRepo
    {
   

        public async Task<User?> LoginUserAsync(User user)
        {
            var u = await _context.Users!.FirstOrDefaultAsync(u => u.Username == user.Username);
            if (u == null)
                return u;

            bool isLoginValid = BCrypt.Net.BCrypt.Verify(user.Password, u!.Password);

            if (isLoginValid)
            {
                 u.LastLogin = DateTime.Now;
            }

            return isLoginValid ? u : null;
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Users!.AnyAsync(x => x.Username == username);
        }


        public Task<User?> GetUserByMobileNumber(string mobile)
        {
            return _context.Users!.FirstOrDefaultAsync(u => u.MobileNumber == mobile);
        }
        public async Task<bool> InUseUsername(User user)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email != user.Email && u.Username == user.Username) != null;
        }

       
    }

}
