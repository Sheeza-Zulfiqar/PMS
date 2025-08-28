using Microsoft.EntityFrameworkCore;
using PMSApi.Data;
using PMSApi.Entities;
using PMSApi.Interfaces.BaseRepos;

namespace PMSApi.Repositories.BaseRepos
{
    public abstract class BaseRepo<T>(ApplicationDbContext context) : IBaseRepo<T>
        where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context = context;

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var entities = await _context
                .Set<T>()
                .OrderByDescending(x => x.CreatedAt)
                 .ToListAsync();
            Console.WriteLine(typeof(T).Name);

            if (entities.Count > 0)
                return entities;

            return await _context
                .Set<T>()
                .IgnoreQueryFilters()
               .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(int id) =>
            await _context.Set<T>().FirstOrDefaultAsync(n => n.Id == id);

 
        public virtual async Task CreateAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            await _context.Set<T>().AddAsync(entity);
        }

        public virtual void Delete(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _context.Set<T>().Remove(entity);
        }

        public virtual async Task SaveChangesAsync(User? user) => await _context.SaveChangesAsync(user);

        public virtual Task<bool> IDExistsAsync(int id) => _context.Set<T>().AnyAsync(e => e.Id == id);

        public virtual async Task<bool> IDsExistAsync(ICollection<int> ids)
        {
            List<int> validIds = await _context.Set<T>().Select(e => e.Id).ToListAsync();
            return ids.All(validIds.Contains);
        }

        public virtual async Task<int> GetCountAsync() => await _context.Set<T>().CountAsync();

        public virtual async Task<T?> GetShallowByIDAsync(int id) =>
            await _context.Set<T>().IgnoreAutoIncludes().FirstOrDefaultAsync(e => e.Id == id);

        public virtual async Task<IEnumerable<T>> GetByCreateUserIDAsync(int userID)
        {
            return await _context.Set<T>().Where(e => e.CreateUserID == userID).ToListAsync();
        }

        public virtual Task<IEnumerable<T>> SearchAsync(string searchQuery)
        {
            throw new NotImplementedException();
        }

        public virtual async Task CreateManyAsync(IEnumerable<T> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);
            await _context.Set<T>().AddRangeAsync(entities);
        }

        public virtual async Task<IEnumerable<T>> GetByIdsAsync(ICollection<int> ids)
        {
            ArgumentNullException.ThrowIfNull(ids);
            return await _context.Set<T>().Where(e => ids.Contains(e.Id)).ToListAsync();
        }
    }

}
