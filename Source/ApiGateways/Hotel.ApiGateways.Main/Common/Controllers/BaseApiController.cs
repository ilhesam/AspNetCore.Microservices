using Hotel.Common.RabbitMq.Manager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Hotel.ApiGateways.Main.Common.Controllers
{
    [ApiController]
    [Route("API/[controller]")]
    public class BaseApiController : ControllerBase
    {
        private IRabbitMqManager _rabbitMq;
        protected IRabbitMqManager RabbitMq => _rabbitMq ??= HttpContext.RequestServices.GetRequiredService<IRabbitMqManager>();
    }
}