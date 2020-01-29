using FootballPredictor.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FootballPredictor.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
       
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ITeamsDbRepository _teamsDbRepository;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
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
