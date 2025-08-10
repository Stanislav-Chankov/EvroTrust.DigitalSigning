using EvroTrust.DigitalSigning.WebApi.Extensions;
using EvroTrust.Infrastructure.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EvroTrust.DigitalSigning.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IRabbitMqPublisherService _publisher;
        private readonly IOptions<RabbitMqOptions> _rabbitMqOptions;

        public InventoryController(IRabbitMqPublisherService publisherService, IOptions<RabbitMqOptions> rabbitMqOptions)
        {
            _publisher = publisherService;
            _rabbitMqOptions = rabbitMqOptions;
        }

        [HttpPost]
        public async Task<IActionResult> RemoveProductFromInventoryAsync()
        {
            await _publisher.InitializeAsync(_rabbitMqOptions.Value);
            await _publisher.PublishAsync("inventory", new CreateOrderCommand
            {
                OrderId = Guid.NewGuid(),
                CustomerName = "Inventory Message",
                TotalAmount = 100.00m
            }.ToJsonString());

            return Ok();
        }
    }
}
