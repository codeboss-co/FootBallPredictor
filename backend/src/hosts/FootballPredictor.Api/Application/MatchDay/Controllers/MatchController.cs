using FootballPredictor.Api.Application.MatchDay.Commands;
using FootballPredictor.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace FootballPredictor.Api.Application.MatchDay.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MatchController : ControllerBase
    {

        private readonly ILogger<MatchController> _logger;
        private readonly ICommandHandler<GetAndStoreMatchData> _matchData;

        public MatchController(
            ILogger<MatchController> logger,
            ICommandHandler<GetAndStoreMatchData> matchData)
        {
            _logger = logger;
            _matchData = matchData;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GetAndStoreMatchData command, CancellationToken token)
        {
            await _matchData.Handle(command, token);

            return Accepted();
        }
    }
}
