using Microsoft.EntityFrameworkCore;
namespace QaDashboardApi.Entities
{
    public class QashboardContext : DbContext
    {
        public QashboardContext(DbContextOptions<QashboardContext> options)
    : base(options)
        {
        }

        public DbSet<Env> Environments { get; set; }
        public DbSet<Platform> Platforms { get; set; }

        public DbSet<Reporter> Reporters { get; set; }

    }
}
