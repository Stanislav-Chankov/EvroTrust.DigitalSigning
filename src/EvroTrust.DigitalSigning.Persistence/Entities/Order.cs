namespace EvroTrust.DigitalSigning.Persistence.Entities
{
    // Create classes for Order, Payment, Shipment, and Product EntityConfigurations with example configurations, properties, reletaions etc.
    public class Order
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public Payment Payment { get; set; } = default!;
        public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}
