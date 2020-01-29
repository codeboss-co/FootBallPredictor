using FootballPredictor.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FootballPredictor.Api.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TeamController : ControllerBase
    {

        private readonly ILogger<TeamController> _logger;
        private readonly ITeamsDbRepository _teamsDbRepository;
        private readonly IMatchDbRepository _matchDbRepository;

        public TeamController(
            ILogger<TeamController> logger,
            ITeamsDbRepository teamsDbRepository,
            IMatchDbRepository matchDbRepository)
        {
            _logger = logger;
            _teamsDbRepository = teamsDbRepository;
            _matchDbRepository = matchDbRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _teamsDbRepository.Query());
        }
    }
}
