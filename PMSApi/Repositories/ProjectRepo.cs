using Microsoft.EntityFrameworkCore;
using PMSApi.Data;
using PMSApi.Entities;
using PMSApi.Interfaces;
using PMSApi.Repositories.BaseRepos;

namespace PMSApi.Repositories
{
    public class ProjectRepo(ApplicationDbContext context) : BaseRepo<Project>(context), IProjectRepo
    {
        public async Task<IEnumerable<Project>> GetMineAsync(int ownerUserId)
        {
            return await _context.Projects
                .Where(p => p.UserId == ownerUserId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Project?> GetOwnedAsync(int id, int ownerUserId)
        {
            return await _context.Projects
                .Include(p => p.ProjectTasks)
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == ownerUserId);
        }

        public async Task<bool> NameExistsForUserAsync(int ownerUserId, string name, int? excludeId = null)
        {
            var q = _context.Projects.Where(p => p.UserId == ownerUserId && p.Name == name);
            if (excludeId is not null) q = q.Where(p => p.Id != excludeId);
            return await q.AnyAsync();
        }
    }

}
