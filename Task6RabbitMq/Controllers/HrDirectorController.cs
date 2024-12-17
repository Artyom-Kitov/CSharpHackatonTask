using Microsoft.AspNetCore.Mvc;
using Task6RabbitMq.Requests;
using Task6RabbitMq.Services;

namespace Task6RabbitMq.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HrDirectorController(IHrDirectorService hrDirectorService) : ControllerBase
    {
        private readonly IHrDirectorService _hrDirectorService = hrDirectorService;

        [HttpPost]
        public void PostWishlists([FromBody] HrDirectorRequest request)
        {
            _hrDirectorService.ReceiveFromHrManager(request);
        }

    }
}
