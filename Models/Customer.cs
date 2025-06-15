// Models/Customer.cs
using System.ComponentModel.DataAnnotations;


namespace GreenGrass.Models
{
    public class Customer
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;
        
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [StringLength(255)]
        public string Street { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string City { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string State { get; set; } = string.Empty;
        
        [StringLength(10)]
        public string ZipCode { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string Country { get; set; } = "USA";
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        
        [StringLength(255)]
        public string? PaymentMethodToken { get; set; }
        
        [StringLength(4)]
        public string? LastFourDigits { get; set; }
        
        public string FullName => $"{FirstName} {LastName}";
        public string FullAddress => $"{Street}, {City}, {State} {ZipCode}".Trim(' ', ',');
    }
}