using EvroTrust.DigitalSigning.Extensions;
using EvroTrust.Infrastructure.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EvroTrust.DigitalSigning.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IRabbitMqPublisherService _publisher;
        private readonly IOptions<RabbitMqOptions> _rabbitMqOptions;

        public OrderController(IRabbitMqPublisherService publisherService, IOptions<RabbitMqOptions> rabbitMqOptions)
        {
            _publisher = publisherService;
            _rabbitMqOptions = rabbitMqOptions;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync()
        {
            await _publisher.InitializeAsync(_rabbitMqOptions.Value);
            await _publisher.PublishAsync("order", new CreateOrderCommand
            {
                OrderId = Guid.NewGuid(),
                CustomerName = "Orders Message",
                TotalAmount = 100.00m
            }.ToJsonString());

            return Ok();
        }
    }
}
