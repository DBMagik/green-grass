namespace GreenGrass.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public Guid ClientId { get; set; }
        public PaymentType Type { get; set; }
        public string CardNumberLast4 { get; set; } = string.Empty;
        public string CardBrand { get; set; } = string.Empty;
        public string ExpiryMonth { get; set; } = string.Empty;
        public string ExpiryYear { get; set; } = string.Empty;
        public string CardHolderName { get; set; } = string.Empty;
        public string PaymentTokenId { get; set; } = string.Empty; // For payment processor
        public bool IsDefault { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

    public enum PaymentType
    {
        CreditCard,
        DebitCard,
        BankAccount,
        Cash,
        Check
    }
}