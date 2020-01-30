using FootballPredictor.Data.Abstractions.Model;
using FootballPredictor.Dto;

namespace FootballPredictor.Domain.Model
{
    public class Match : IAggregateRoot<long>
    {
        private const string HOME_TEAM = "HOME_TEAM";
        private const string AWAY_TEAM = "AWAY_TEAM";
        private const string DRAW = "DRAW";

        public long Id { get; set; }
        public long MatchId { get; set; }
        public long SeasonId { get; set; }
        public long Matchday { get; set; }
        public string HomeTeam { get; set; }
        public long HomeTeamId { get; set; }
        public string AwayTeam { get; set; }
        public long AwayTeamId { get; set; }
        public string Winner { get; set; }
        public long? WinnerId { get; set; }
        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }

        // ORM required
        private Match()
        {
        }

        public Match(MatchDto dto)
        {
            MatchId = dto.Id;
            SeasonId = dto.Season.Id;
            Matchday = dto.Matchday;
            Winner = dto.Score.Winner;
            HomeTeam = dto.HomeTeam.Name;
            HomeTeamId = dto.HomeTeam.Id;
            AwayTeam = dto.AwayTeam.Name;
            AwayTeamId = dto.AwayTeam.Id;
            HomeTeamGoals = dto.Score.FullTime.HomeTeam;
            AwayTeamGoals = dto.Score.FullTime.AwayTeam;
            SetWinnerId(dto);
        }

        private void SetWinnerId(MatchDto dto)
        {
            switch (dto.Score.Winner)
            {
                case HOME_TEAM:
                    WinnerId = HomeTeamId;
                    break;
                case AWAY_TEAM:
                    WinnerId = AwayTeamId;
                    break;
                default:
                    WinnerId = null;
                    break;
            }
        }
    }
}
