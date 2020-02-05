using FootballPredictor.Common;
using FootballPredictor.Domain.Services;
using FootballPredictor.Dto;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace FootballPredictor.Api.Application.MatchDay.Queries
{
    #region Query and Result
    public class QueryGetMatchDayFixtures : IQuery<MatchFixtures>
    {
        public string Competition { get; set; } = "PL";
        public int Matchday { get; set; } = 1;
        public int? Season { get; set; } = 2020;
    }

    public class MatchFixtures : Collection<MatchFixtureDto>
    {
    } 
    #endregion

    public class GetMatchDayFixturesHandler : IQueryHandler<QueryGetMatchDayFixtures, MatchFixtures>
    {
        private readonly IMatchDataProvider _matchDataProvider;

        public GetMatchDayFixturesHandler(IMatchDataProvider matchDataProvider)
        {
            _matchDataProvider = matchDataProvider;
        }

        public async ValueTask<MatchFixtures> Handle(QueryGetMatchDayFixtures query, CancellationToken token = default)
        {
            var matchData = await _matchDataProvider.GetMatchDayDataAsync(
                    competition: query.Competition,
                    matchday: query.Matchday,
                    season: query.Season,
                    token)
                .ConfigureAwait(false);

            var fixtures = new MatchFixtures();
            matchData.Matches.ForEach( match => fixtures.Add(new MatchFixtureDto
            {
                HomeTeam = match.HomeTeam.Name,
                AwayTeam = match.AwayTeam.Name,
            }));

            return fixtures;
        }
    }
}
