using Microsoft.EntityFrameworkCore;
using Services.Models;

namespace Services.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Country>? Countries { get; set; }
        public DbSet<Subscription>? Subscriptions { get; set; }
        public DbSet<ClientUser>? ClientUsers { get; set; }
        public DbSet<Movie>? Movies { get; set; }
        public DbSet<Rent>? Rents { get; set; }
        public DbSet<RentItem>? RentItems { get; set; }
        public DbSet<PreRent>? PreRents { get; set; }
        public DbSet<PreRentItem>? PreRentItems { get; set; }
        public DbSet<Genre>? Genres { get; set; }
        public DbSet<RentStatus>? RentStatuses { get; set; }
        public DbSet<AdminUser>? AdminUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Subscription>().HasIndex(s => s.Name).IsUnique();
            modelBuilder.Entity<ClientUser>().HasIndex(cu => cu.Email).IsUnique();
            modelBuilder.Entity<Movie>().HasIndex(m => m.FirebasePosterId).IsUnique();
            modelBuilder.Entity<Genre>().HasIndex(g => g.Name).IsUnique();
            modelBuilder.Entity<AdminUser>().HasIndex(g => g.Email).IsUnique();
        }
    }
}