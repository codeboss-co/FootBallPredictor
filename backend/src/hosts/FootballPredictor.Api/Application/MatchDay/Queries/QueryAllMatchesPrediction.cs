using FootballPredictor.Domain.Services;
using FootballPredictorML.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FootballPredictor.Dto;

namespace FootballPredictor.Api.Application.MatchDay.Queries
{
    public class QueryAllMatchesPrediction : IQuery<IEnumerable<MatchDayPrediction>>
    {
        public string Competition { get; set; } = "PL";
        public int Matchday { get; set; } = 1;
        public int? Season { get; set; } = 2020;
    }

    public class MatchDayPrediction : MatchFixtureDto
    {
        public IDictionary<string, float> Prediction { get; set; }
    }

    public class PredictAllMatchesQueryHandler : IQueryHandler<QueryAllMatchesPrediction, IEnumerable<MatchDayPrediction>>
    {
        private readonly IQueryHandler<QueryGetMatchDayFixtures, MatchFixtures> _fixturesQuery;

        public PredictAllMatchesQueryHandler(IQueryHandler<QueryGetMatchDayFixtures, MatchFixtures> _fixturesQuery)
        {
            this._fixturesQuery = _fixturesQuery;
        }


        public async ValueTask<IEnumerable<MatchDayPrediction>> Handle(QueryAllMatchesPrediction query, CancellationToken token = default)
        {
            var fixtureQuery = new QueryGetMatchDayFixtures
            {
                Competition = query.Competition,
                Matchday = query.Matchday,
                Season = query.Season
            };

            var predictions = new List<MatchDayPrediction>();
            foreach (var match in await _fixturesQuery.Handle(fixtureQuery, token))
            {
                var prediction = ConsumeModel.Predict(new ModelInput
                {
                    HomeTeam = match.HomeTeam,
                    AwayTeam = match.AwayTeam
                });

                predictions.Add(new MatchDayPrediction()
                {
                    HomeTeam = match.HomeTeam,
                    AwayTeam = match.AwayTeam,
                    Prediction = new Dictionary<string, float>
                    {
                        [match.HomeTeam] = prediction.Score[0] * 100,
                        [match.AwayTeam] = prediction.Score[1] * 100,
                        ["Draw"] = prediction.Score[2] * 100,
                    }
                });
            }

            return predictions;
        }
    }
}
