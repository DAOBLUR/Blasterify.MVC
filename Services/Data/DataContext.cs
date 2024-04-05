using Microsoft.EntityFrameworkCore;
using Services.Models;

namespace Services.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<ClientUser> ClientUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Subscription>().HasIndex(s => s.Name).IsUnique();
            modelBuilder.Entity<ClientUser>().HasIndex(cu => cu.Email).IsUnique();
        }
    }
}