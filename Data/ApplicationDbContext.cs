// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using GreenGrass.Models;  // For reference to models if needed


namespace GreenGrass.Data
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;  // Default to empty or handle as needed
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        public string? PaymentMethodToken { get; set; }
        public string? LastFourDigits { get; set; }
    }


    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;


        public ApplicationDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public DbSet<Customer> Customers { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            // Additional fluent API configurations can be added if needed (e.g., for indexes)
        }
    }
}