using PMSApi.Entities;

namespace PMSApi.Interfaces.BaseRepos
{
    public interface IBaseRepo<T>
     where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetByIdsAsync(ICollection<int> ids);
        Task CreateAsync(T entity);
        Task CreateManyAsync(IEnumerable<T> entities);
        void Delete(T entity);
        Task SaveChangesAsync(User? user);
        //Task<PagedResponse<T>> GetPagedAsync(int page, int pageSize);
        Task<bool> IDExistsAsync(int id);
        Task<bool> IDsExistAsync(ICollection<int> ids);
        Task<int> GetCountAsync();
        Task<T?> GetShallowByIDAsync(int id);
        Task<IEnumerable<T>> GetByCreateUserIDAsync(int userID);
        Task<IEnumerable<T>> SearchAsync(string searchQuery);
    }

}
