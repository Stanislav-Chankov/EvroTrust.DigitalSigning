
namespace EvroTrust.DigitalSigning.WebApi.Controllers
{
    internal class CreateOrderCommand
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
    }
}