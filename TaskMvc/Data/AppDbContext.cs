using Microsoft.EntityFrameworkCore;
using TaskMvc.Models;

namespace TaskMvc.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasKey(e => e.PayrollNumber); // primary key
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
