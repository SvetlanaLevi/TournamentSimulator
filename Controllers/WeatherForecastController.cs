using Microsoft.AspNetCore.Mvc;

namespace TournamentSimulator.Controllers
{
    [ApiController]
    [Route("simulate")]
    public class TournamentController : ControllerBase
    {
        [HttpPost]
        public IActionResult Simulate()
        {
            return Ok();
        }
    }
}
