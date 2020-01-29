using FootballPredictor.Domain.Model;
using FootballPredictor.Domain.Repositories;
using FootballPredictor.Domain.Services;
using System.Threading;
using System.Threading.Tasks;

namespace FootballPredictor.Api.Application.MatchDay.Commands
{
    public class GetAndStoreMatchData
    {
        public string Competition { get; set; } = "PL";
        public int Matchday { get; set; } = 1;
    }

    public class GetAndStoreMatchDataCommandHandler : ICommandHandler<GetAndStoreMatchData>
    {
        private readonly IMatchDataProvider _matchDataProvider;
        private readonly IMatchDbRepository _matchDbRepository;

        public GetAndStoreMatchDataCommandHandler(
            IMatchDataProvider matchDataProvider,
            IMatchDbRepository matchDbRepository)
        {
            _matchDataProvider = matchDataProvider;
            _matchDbRepository = matchDbRepository;
        }

        public async Task Handle(GetAndStoreMatchData command, CancellationToken token = default)
        {
            var matchData = await _matchDataProvider.GetMatchDayDataAsync(
                                                    competition:command.Competition, 
                                                    matchday:command.Matchday, 
                                                    accessToken:"eb80d76def304f6fb82a123b1fd826a1")
                                                .ConfigureAwait(false);

            foreach (var matchDto in matchData.Matches)
            {
                var match = new Match
                {
                    MatchId = matchDto.Id,
                    SeasonId = matchDto.Season.Id,
                    Matchday = matchDto.Matchday,
                    Winner = matchDto.Score.Winner,
                    HomeTeam = matchDto.HomeTeam.Name,
                    AwayTeam = matchDto.AwayTeam.Name,
                    HomeTeamGoals = matchDto.Score.FullTime.HomeTeam,
                    AwayTeamGoals = matchDto.Score.FullTime.AwayTeam
                };

                await _matchDbRepository.InsertAsync(match, token).ConfigureAwait(false);
            }
        }
    }
}
