using FootballPredictor.Data.Abstractions.Model;

namespace FootballPredictor.Domain.Model
{
    public class Match : IAggregateRoot<long>
    {
        public long Id { get; set; }
        public long MatchId { get; set; }
        public long SeasonId { get; set; }
        public long Matchday { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string Winner { get; set; }
        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }
    }
}
