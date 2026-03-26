using Microsoft.AspNetCore.Mvc;
using TournamentSimulator.Logic;

namespace TournamentSimulator.Controllers
{
    [ApiController]
    [Route("tournament")]
    public class TournamentController : ControllerBase
    {
        [HttpGet("simulate")]
        public IActionResult Simulate([FromServices] ITournamentService service)
        {
            var simulationResult = service.Simulate();
            return Ok(simulationResult);
        }
    }
}
