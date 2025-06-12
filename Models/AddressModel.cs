// Models/Address.cs
using System.ComponentModel.DataAnnotations;

namespace GreenGrass.Models
{
    public class Address
    {
        [Required]
        [StringLength(200)]
        public string Street { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;
        
        [Required]
        [StringLength(10)]
        public string State { get; set; } = string.Empty;
        
        [Required]
        [StringLength(10)]
        public string ZipCode { get; set; } = string.Empty;
        
        public string FullAddress => $"{Street}, {City}, {State} {ZipCode}";
    }
}