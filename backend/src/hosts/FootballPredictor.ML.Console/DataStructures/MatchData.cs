using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace FootballPredictor.ML.Console.DataStructures
{
    /// <summary>
    /// The MatchData class represents a single Football Match.
    /// </summary>
    public class MatchData
    {
        public float SeasonId { get; set; }
        public string HomeTeam { get; set; }
        public float HomeTeamId { get; set; }
        public string AwayTeam { get; set; }
        public float AwayTeamId { get; set; }
        public string Winner { get; set; }
        public float WinnerId { get; set; }
        public float HomeTeamGoals { get; set; }
        public float AwayTeamGoals { get; set; }
    }

    public class FootBallMatchPrediction
    {
        [ColumnName("Score")]
        public float WinnerId;
    }
}
