using System.Collections.Generic;
using FootballPredictor.Api.Application.MatchDay.Commands;
using FootballPredictor.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using FootballPredictor.Api.Application.MatchDay.Queries;
using FootballPredictor.Dto;
using FootballPredictorML.Model;

namespace FootballPredictor.Api.Application.MatchDay.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MatchController : ControllerBase
    {

        private readonly ICommandHandler<GetAndStoreMatchData> _matchData;
        private readonly IQueryHandler<QueryGetMatchDayFixtures, MatchFixtures> _fixturesQuery;

        public MatchController(
            ICommandHandler<GetAndStoreMatchData> matchData,
            IQueryHandler<QueryGetMatchDayFixtures, MatchFixtures> fixturesQuery)
        {
            _matchData = matchData;
            _fixturesQuery = fixturesQuery;
        }

        [HttpGet]
        public async Task<IActionResult> GetMatchDayFixtures([FromBody] QueryGetMatchDayFixtures query, CancellationToken token)
        {
            return Ok(await _fixturesQuery.Handle(query, token));
        }

        [HttpPost]
        public async Task<IActionResult> GetAndStoreMatchData([FromBody] GetAndStoreMatchData command, CancellationToken token)
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
