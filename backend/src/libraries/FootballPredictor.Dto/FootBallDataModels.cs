using System;
using System.Collections.Generic;

namespace FootballPredictor.Dto
{
    public class MatchDayData
    {
        public int Count { get; set; }
        public Competition Competition { get; set; }
        public IList<MatchDto> Matches { get; set; }
    }

    public class MatchDto
    {
        public int Id { get; set; }
        public Season Season { get; set; }
        public DateTime UtcDate { get; set; }
        public string Status { get; set; }
        public int Matchday { get; set; }
        public string Stage { get; set; }
        public string Group { get; set; }
        public DateTime LastUpdated { get; set; }
        public Score Score { get; set; }
        public TeamDto HomeTeam { get; set; }
        public TeamDto AwayTeam { get; set; }
    }

    public class Competition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Plan { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class Season
    {
        public int Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int CurrentMatchday { get; set; }
    }

    public class FullTime
    {
        public int? HomeTeam { get; set; }
        public int? AwayTeam { get; set; }
    }
    
    public class Score
    {
        public string Winner { get; set; }
        public FullTime FullTime { get; set; }
    }


    public class TeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    

}
