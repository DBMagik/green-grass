// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using GreenGrass.Models;


namespace GreenGrass.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


        public DbSet<Customer> Customers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.Street).HasMaxLength(255);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.State).HasMaxLength(50);
                entity.Property(e => e.ZipCode).HasMaxLength(10);
                entity.Property(e => e.Country).HasMaxLength(100);
                entity.Property(e => e.PaymentMethodToken).HasMaxLength(255);
                entity.Property(e => e.LastFourDigits).HasMaxLength(4);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}