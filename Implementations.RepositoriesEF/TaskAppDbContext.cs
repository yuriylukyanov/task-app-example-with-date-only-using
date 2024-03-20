using Implementations.RepositoriesEF.Entitites;
using Microsoft.EntityFrameworkCore;

namespace Implementations.RepositoriesEF
{
    public class TaskAppDbContext: DbContext
    {
        public TaskAppDbContext(DbContextOptions<TaskAppDbContext> options): base(options)
        { 
        }

        public DbSet<TaskEntity> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }

    }
}