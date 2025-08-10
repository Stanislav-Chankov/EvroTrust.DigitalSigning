namespace EvroTrust.DigitalSigning.Persistence.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
