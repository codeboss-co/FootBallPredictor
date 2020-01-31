using FootballPredictor.ML.Console.DataStructures;
using Microsoft.ML;
using Microsoft.ML.Data;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;


namespace FootballPredictor.ML.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var connString = @"Host=rogue.db.elephantsql.com;Database=hhgfafoj;Username=hhgfafoj;Password=bxK--h2kwjxoblpg2JsN1AYIHLef0012";
            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();
            conn.Close();

            System.Console.Title = typeof(Program).Namespace.Split(".").First();

            //STEP 1: Create MLContext to be shared across the model creation workflow objects 
            var mlContext = new MLContext();

            ////STEP 2:  Load data from a relational database requires  System.Data.SqlClient NuGet package
            DatabaseLoader loader = mlContext.Data.CreateDatabaseLoader<MatchData>();
            string connectionString = @"Host=rogue.db.elephantsql.com;Database=hhgfafoj;Username=hhgfafoj;Password=bxK--h2kwjxoblpg2JsN1AYIHLef0012";
            // Numerical data that is not of type Real has to be converted to Real
            string sqlCommand = "SELECT \r\nCAST(\"public\".\"Matches\".\"SeasonId\" as REAL) as \"SeasonId\", \r\n\"public\".\"Matches\".\"HomeTeam\",\r\nCAST(\"public\".\"Matches\".\"HomeTeamId\" as REAL) as \"HomeTeamId\", \r\n\"public\".\"Matches\".\"AwayTeam\",\r\nCAST(\"public\".\"Matches\".\"AwayTeamId\" as REAL) as \"AwayTeamId\", \r\n\"public\".\"Matches\".\"Winner\",\r\nCAST(\"public\".\"Matches\".\"WinnerId\" as REAL) as \"WinnerId\", \r\nCAST(\"public\".\"Matches\".\"HomeTeamGoals\" as REAL) as \"HomeTeamGoals\", \r\nCAST(\"public\".\"Matches\".\"AwayTeamGoals\" as REAL) as \"AwayTeamGoals\"\r\nFROM \"public\".\"Matches\"";
            var dbSource = new DatabaseSource(NpgsqlFactory.Instance, connectionString, sqlCommand);

            // load the data 
            System.Console.WriteLine("Loading training data....");
            var dataView = loader.Load(dbSource);
            System.Console.WriteLine("done");

            
            // set up a learning pipeline
            var pipeline = mlContext.Transforms.CopyColumns(
                    inputColumnName: "WinnerId",
                    outputColumnName: "Label")

                // one-hot encode all text features
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("HomeTeamId"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding("AwayTeamId"))

                // combine all input features into a single column 
                .Append(mlContext.Transforms.Concatenate(
                    "Features",
                    "HomeTeamId",
                    "AwayTeamId")

                // cache the data to speed up training
                .AppendCacheCheckpoint(mlContext)

                // use the fast tree learner 
                // requires installation of additional NuGet package: Microsoft.ML.FastTree
                .Append(mlContext.Regression.Trainers.FastTree()));

            // train the model
            System.Console.WriteLine("Training the model....");
            var model = pipeline.Fit(dataView);
            System.Console.WriteLine("done");

            // get a set of predictions 
            var predictions = model.Transform(dataView);

            // get regression metrics to score the model
            var metrics = mlContext.Regression.Evaluate(predictions, "Label", "Score");

            // show the metrics
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine($"*************************************************");
            System.Console.WriteLine($"*       Model quality metrics evaluation         ");
            System.Console.WriteLine($"*------------------------------------------------");
            System.Console.WriteLine($"*       RSquared Score:      {metrics.RSquared:0.##}");// The closer its value is to 1, the better the model is.
            System.Console.WriteLine($"*       Root Mean Squared Error:      {metrics.RootMeanSquaredError:#.##}"); // The lower it is, the better the model is. 

            // PREDICTIONS
            var predictionFunction = mlContext.Model.CreatePredictionEngine<MatchData, FootBallMatchPrediction>(model);

            // prep a single taxi trip
            var matchDataSample = new MatchData()
            {
                HomeTeamId = 64,
                AwayTeamId = 68,
                WinnerId = 0 // actual winner for this match = 64
            };

            // make the prediction
            var prediction = predictionFunction.Predict(matchDataSample);

            // show the prediction
            System.Console.WriteLine();
            System.Console.WriteLine($"*************************************************");
            System.Console.WriteLine($"Single prediction:");
            System.Console.WriteLine($"  Predicted winner: {prediction.WinnerId}");
            System.Console.WriteLine($"  Actual winner: 64");
        }
    }
}
