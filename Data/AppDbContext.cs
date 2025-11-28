using CustomerRegistration.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerRegistration.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<OtpRequest> OtpRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.ICNumber)
                .IsUnique();

            modelBuilder.Entity<OtpRequest>()
                .HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.CustomerId);
        }
    }
}
