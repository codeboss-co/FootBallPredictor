using System.Collections.Generic;
using FootballPredictor.Domain.Model;
using FootballPredictor.Domain.Repositories;
using FootballPredictor.Domain.Services;
using System.Threading;
using System.Threading.Tasks;
using FootballPredictor.Common;

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
                                                    token)
                                                .ConfigureAwait(false);

            var matchList = new List<Match>(matchData.Matches.Count);
            foreach (var matchDto in matchData.Matches)
            {
                matchList.Add(new Match(matchDto));
            }

            if (!matchList.IsNullOrEmpty())
            {
                await _matchDbRepository.InsertManyAsync(matchList, token).ConfigureAwait(false);
            }
        }
    }
}
