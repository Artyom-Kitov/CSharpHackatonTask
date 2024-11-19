using Microsoft.AspNetCore.Mvc;
using Task5Http.Requests;
using Task5Http.Services;

namespace Task5Http.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HrDirectorController(IHrDirectorService hrDirectorService) : ControllerBase
    {
        private readonly IHrDirectorService _hrDirectorService = hrDirectorService;

        [HttpPost]
        public void PostWishlists([FromBody] HrDirectorRequest request)
        {
            _hrDirectorService.ReceiveFromHrManager(request.Teams, request.JuniorWishlists, request.TeamleadWishlists);
        }
    }
}
