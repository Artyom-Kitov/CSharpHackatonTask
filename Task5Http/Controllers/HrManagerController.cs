using Microsoft.AspNetCore.Mvc;
using Nsu.HackathonProblem.Contracts;
using Task5Http.Services;

namespace Task5Http.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HrManagerController(IHrManagerService hrManagerService) : ControllerBase
    {
        private readonly IHrManagerService _hrManagerService = hrManagerService;

        [HttpPost("junior")]
        public IActionResult PostJuniorWishlist([FromBody] Wishlist wishlist)
        {
            _hrManagerService.SaveJuniorPreferences(wishlist);
            return Ok();
        }

        [HttpPost("teamlead")]
        public IActionResult PostTeamleadWishlist([FromBody] Wishlist wishlist)
        {
            _hrManagerService.SaveTeamleadPreferences(wishlist);
            return Ok();
        }
    }
}
