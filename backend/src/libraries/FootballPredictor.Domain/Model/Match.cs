using FootballPredictor.Data.Abstractions.Model;
using FootballPredictor.Dto;

namespace FootballPredictor.Domain.Model
{
    public class Match : IAggregateRoot<long>
    {
        public long Id { get; set; }
        public long SeasonId { get; set; }
        public long Matchday { get; set; }
        public Winner Winner { get; set; }
        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }
    }
}
