using System.Collections.Generic;
using FootballPredictor.Domain.Model;
using FootballPredictor.Domain.Repositories;
using FootballPredictor.Domain.Services;
using System.Threading;
using System.Threading.Tasks;
using FootballPredictor.Common;
using Serilog;

namespace FootballPredictor.Api.Application.MatchDay.Commands
{
    public class GetAndStoreMatchData
    {
        public string Competition { get; set; } = "PL";
        public int Matchday { get; set; } = 1;
        public int? Season { get; set; } = 2020;
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
                                                    season:command.Season,
                                                    token)
                                                .ConfigureAwait(false);

            var matchList = new List<Match>(matchData.Matches.Count);
            matchData.Matches.ForEach(matchDto => matchList.Add(new Match(matchDto)));
            
            if (!matchList.IsNullOrEmpty())
            {
                Log.Information("Inserting match data into database.");
                await _matchDbRepository.InsertManyAsync(matchList, token).ConfigureAwait(false);
                Log.Information("Match data successfully inserted: {matches}", matchList.Count);
            }
            else
            {
                Log.Information("No match data to insert into database.");
            }
        }
    }
}
