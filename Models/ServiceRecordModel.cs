using System;
using System.ComponentModel.DataAnnotations;

namespace GreenGrass.Models
{
    public class ServiceRecord
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        
        [Required]
        public ServiceType ServiceType { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public decimal Amount { get; set; }
        
        public DateTime ServiceDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? CompletedDate { get; set; }

        public ServiceStatus Status { get; set; } = ServiceStatus.Pending;
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        public string? PaymentTransactionId { get; set; }
        public DateTime? PaymentDate { get; set; }
        
        public string? Notes { get; set; }
        
        // Navigation property
        public Client Client { get; set; } = null!;
    }
    
    public enum ServiceType
    {
        LawnMowing,
        Landscaping,
        TreeTrimming,
        Fertilization,
        WeedControl,
        Cleanup,
        Planting,
        Other
    }
    
    public enum ServiceStatus
    {
        Pending,
        InProgress,
        Completed,
        Cancelled
    }
    
    public enum PaymentStatus
    {
        Pending,
        Paid,
        Failed,
        Refunded,
        Processing
    }
}