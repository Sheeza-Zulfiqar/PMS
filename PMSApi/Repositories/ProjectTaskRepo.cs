using Microsoft.EntityFrameworkCore;
using PMSApi.Data;
using PMSApi.Entities;
using PMSApi.Interfaces;
using PMSApi.Repositories.BaseRepos;

namespace PMSApi.Repositories
{
    public class ProjectTaskRepo(ApplicationDbContext context) : BaseRepo<ProjectTasks>(context), IProjectTaskRepo
    {
        public async Task<IEnumerable<ProjectTasks>> GetForProjectAsync(int projectId, int ownerUserId)
        {
             var owned = await _context.Projects.AnyAsync(p => p.Id == projectId && p.UserId == ownerUserId);
            if (!owned) return Enumerable.Empty<ProjectTasks>();

            return await _context.ProjectTasks
                .Where(t => t.ProjectId == projectId)
                .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
        }

        public async Task<ProjectTasks?> GetOwnedTaskAsync(int taskId, int ownerUserId)
        {
            return await _context.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == taskId && t.Project.UserId == ownerUserId);
        }
    }
}
