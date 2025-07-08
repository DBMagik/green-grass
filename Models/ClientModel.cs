using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace GreenGrass.Models
{
    public class Client
    {
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [Required]
        public Address Address { get; set; } = new();
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        
        // Payment information (encrypted/tokenized)
        public string? PaymentMethodToken { get; set; }
        public string? LastFourDigits { get; set; }
        
        // Navigation properties
        public List<ServiceRecord> ServiceHistory { get; set; } = [];
        public List<PaymentMethod> PaymentMethods { get; set; } = [];
        
        public string FullName => $"{FirstName} {LastName}";
        
        // Helper property to get default payment method
        public PaymentMethod? DefaultPaymentMethod => PaymentMethods.FirstOrDefault(p => p.IsDefault);
    }
}