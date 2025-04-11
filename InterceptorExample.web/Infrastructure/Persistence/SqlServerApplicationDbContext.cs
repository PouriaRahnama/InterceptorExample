using InterceptorExample.web.Domain;
using Microsoft.EntityFrameworkCore;

namespace InterceptorExample.web.Infrastructure.Persistence
{
    public class SqlServerApplicationDbContext:DbContext
    {
        public SqlServerApplicationDbContext(DbContextOptions<SqlServerApplicationDbContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.OnCreated();
            
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Link> Links { get; set; }  
    }
}
