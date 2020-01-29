using System;
using System.Collections.Generic;

namespace FootballPredictor.Dto
{
    public partial class MatchDayData
    {
        public long Count { get; set; }  // Matches Played
        public Filters Filters { get; set; }
        public Competition Competition { get; set; }
        public List<MatchDto> Matches { get; set; }
    }

    public partial class Competition
    {
        public long Id { get; set; }
        public string Name { get; set; }  // Premier League
        public string Code { get; set; }  // PL
        public DateTimeOffset LastUpdated { get; set; }
    }

    public partial class KeyValuePair
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public partial class Filters
    {
        public long Matchday { get; set; }
    }

    public partial class MatchDto
    {
        public long Id { get; set; }
        public Season Season { get; set; }
        public DateTimeOffset UtcDate { get; set; }
        public long Matchday { get; set; }
        public DateTimeOffset LastUpdated { get; set; }
        public Score Score { get; set; }
        public KeyValuePair HomeTeam { get; set; }
        public KeyValuePair AwayTeam { get; set; }
    }

    public partial class Score
    {
        public Winner Winner { get; set; }
        public FullTimeScore FullTime { get; set; }
    }

    public partial class FullTimeScore
    {
        public int HomeTeam { get; set; }
        public int AwayTeam { get; set; }
    }

    public partial class Season
    {
        public long Id { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public long CurrentMatchday { get; set; }
    }


    public enum Winner { AwayTeam, Draw, HomeTeam };

}
