using FootballPredictor.Api.Application.MatchDay.Commands;
using FootballPredictor.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using FootballPredictorML.Model;

namespace FootballPredictor.Api.Application.MatchDay.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MatchController : ControllerBase
    {

        private readonly ICommandHandler<GetAndStoreMatchData> _matchData;

        public MatchController(ICommandHandler<GetAndStoreMatchData> matchData)
        {
            _matchData = matchData;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GetAndStoreMatchData command, CancellationToken token)
        {
            await _matchData.Handle(command, token);

            return Ok();
        }

        [HttpGet("predict")]
        public async Task<IActionResult> Predict([FromBody] PredictMatchOutcome command, CancellationToken token)
        {
            var prediction = ConsumeModel.Predict(new ModelInput
            {
                HomeTeam = command.HomeTeam,
                AwayTeam = command.AwayTeam
            });

            return Ok(prediction);
        }
    }
}
