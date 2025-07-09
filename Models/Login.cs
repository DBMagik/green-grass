using System.ComponentModel.DataAnnotations;

namespace GreenGrass.Models
{
    public class Login
    {
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public byte[] PasswordHash { get; set; } = new byte[64];
        
        [Required]
        public byte[] Salt { get; set; } = new byte[32];
    }
}