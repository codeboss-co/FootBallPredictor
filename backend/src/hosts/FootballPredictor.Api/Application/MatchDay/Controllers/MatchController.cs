using System.Collections.Generic;
using FootballPredictor.Api.Application.MatchDay.Commands;
using FootballPredictor.Api.Application.MatchDay.Queries;
using FootballPredictor.Domain.Services;
using FootballPredictorML.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace FootballPredictor.Api.Application.MatchDay.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MatchController : ControllerBase
    {

        private readonly ICommandHandler<GetAndStoreMatchData> _matchData;
        private readonly IQueryHandler<QueryGetMatchDayFixtures, MatchFixtures> _fixturesQuery;
        private readonly IQueryHandler<QueryAllMatchesPrediction, IEnumerable<MatchDayPrediction>> _predictor;

        public MatchController(
            ICommandHandler<GetAndStoreMatchData> matchData,
            IQueryHandler<QueryGetMatchDayFixtures, MatchFixtures> fixturesQuery,
            IQueryHandler<QueryAllMatchesPrediction, IEnumerable<MatchDayPrediction>> predictor)
        {
            _matchData = matchData;
            _fixturesQuery = fixturesQuery;
            _predictor = predictor;
        }

        [HttpGet("fixtures")]
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

        [HttpPost("update-model")]
        public async Task<IActionResult> UpdateModel(CancellationToken token)
        {
            FootballPredictorML.ConsoleApp.ModelBuilder.CreateModel();

            return Ok();
        }

        [HttpGet("predict")]
        public async Task<IActionResult> Predict([FromBody] PredictMatch command, CancellationToken token)
        {
            var prediction = ConsumeModel.Predict(new ModelInput
            {
                HomeTeam = command.HomeTeam,
                AwayTeam = command.AwayTeam
            });

            return Ok(prediction);
        }

        [HttpGet("predict-all")]
        public async Task<IActionResult> PredictAllMatches([FromBody] QueryAllMatchesPrediction command, CancellationToken token)
        {
            return Ok(await _predictor.Handle(command, token));
        }
    }
}
