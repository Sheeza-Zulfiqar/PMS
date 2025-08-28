using PMSApi.Entities;
using PMSApi.Interfaces.BaseRepos;

namespace PMSApi.Interfaces
{
    public interface IProjectRepo : IBaseRepo<Project>
    {
        Task<IEnumerable<Project>> GetMineAsync(int ownerUserId);
        Task<Project?> GetOwnedAsync(int id, int ownerUserId);
        Task<bool> NameExistsForUserAsync(int ownerUserId, string name, int? excludeId = null);
    }
}
