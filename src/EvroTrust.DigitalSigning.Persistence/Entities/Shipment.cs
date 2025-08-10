namespace EvroTrust.DigitalSigning.Persistence.Entities
{
    public class Shipment
    {
        public int OrderId { get; set; }
        public string TrackingNumber { get; set; } = string.Empty;
        public DateTime ShipmentDate { get; set; }
        public Order Order { get; set; } = default!;
    }
}
