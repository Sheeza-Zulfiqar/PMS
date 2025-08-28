using PMSApi.Entities;
using PMSApi.Interfaces.BaseRepos;

namespace PMSApi.Interfaces
{
    public interface IProjectTaskRepo : IBaseRepo<ProjectTasks>
    {
        Task<IEnumerable<ProjectTasks>> GetForProjectAsync(int projectId, int ownerUserId);
        Task<ProjectTasks?> GetOwnedTaskAsync(int taskId, int ownerUserId);
    }

}
