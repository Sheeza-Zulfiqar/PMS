using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using PMSApi.Entities;

namespace PMSApi.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
     : DbContext(options)
    {

        
        
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTasks> ProjectTasks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();
            modelBuilder.Entity<User>().HasOne(x => x.CreatedBy);
            modelBuilder.Entity<User>().HasOne(x => x.UpdatedBy);



        }
        public async Task<int> SaveChangesAsync(User? user)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is BaseEntity
                    && (e.State == EntityState.Added || e.State == EntityState.Modified)
                );

            foreach (var entityEntry in entries)
            {
                if (
                    entityEntry.State == EntityState.Added
                    || ((BaseEntity)entityEntry.Entity).CreatedAt == null
                )
                {
                    ((BaseEntity)entityEntry.Entity).CreatedAt =
                        ((BaseEntity)entityEntry.Entity).CreatedAt ?? DateTime.Now;
                    ((BaseEntity)entityEntry.Entity).CreateUserID =
                        ((BaseEntity)entityEntry.Entity).CreateUserID
                        ?? (user?.Id > 0 ? user.Id : null);
                }

                ((BaseEntity)entityEntry.Entity).UpdatedAt =
                    ((BaseEntity)entityEntry.Entity).UpdatedAt ?? DateTime.Now;
                ((BaseEntity)entityEntry.Entity).UpdateUserID =
                    ((BaseEntity)entityEntry.Entity).UpdateUserID ?? (user?.Id > 0 ? user.Id : null);
            }

            return await base.SaveChangesAsync(new CancellationToken());
        }

    }
}
