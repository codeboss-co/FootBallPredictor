using FootballPredictor.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FootballPredictor.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TeamController : ControllerBase
    {
       
        private readonly ILogger<TeamController> _logger;
        private readonly ITeamsDbRepository _teamsDbRepository;

        public TeamController(
            ILogger<TeamController> logger,
            ITeamsDbRepository teamsDbRepository
            )
        {
            _logger = logger;
            _teamsDbRepository = teamsDbRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _teamsDbRepository.Query());
        }
    }
}
