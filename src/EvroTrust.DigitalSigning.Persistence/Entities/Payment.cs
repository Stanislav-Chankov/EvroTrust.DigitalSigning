namespace EvroTrust.DigitalSigning.Persistence.Entities
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public Order Order { get; set; } = default!;
    }
}
