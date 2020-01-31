using System;
using System.Collections.Generic;
using FootballPredictor.ML.Console.DataStructures;
using Microsoft.ML;
using Microsoft.ML.Data;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;
using FootballPredictorML.Model;


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
            string sqlCommand = "SELECT \r\nCAST(\"public\".\"Matches\".\"Id\" as REAL) as \"Id\",\r\nCAST(\"public\".\"Matches\".\"MatchId\" as REAL) as \"MatchId\", \r\nCAST(\"public\".\"Matches\".\"SeasonId\" as REAL) as \"SeasonId\", \r\n\"public\".\"Matches\".\"Matchday\",\r\n\"public\".\"Matches\".\"HomeTeam\",\r\nCAST(\"public\".\"Matches\".\"HomeTeamId\" as REAL) as \"HomeTeamId\", \r\n\"public\".\"Matches\".\"AwayTeam\",\r\nCAST(\"public\".\"Matches\".\"AwayTeamId\" as REAL) as \"AwayTeamId\", \r\n\"public\".\"Matches\".\"Winner\",\r\nCAST(\"public\".\"Matches\".\"WinnerId\" as REAL) as \"WinnerId\", \r\nCAST(\"public\".\"Matches\".\"HomeTeamGoals\" as REAL) as \"HomeTeamGoals\", \r\nCAST(\"public\".\"Matches\".\"AwayTeamGoals\" as REAL) as \"AwayTeamGoals\"\r\nFROM \"public\".\"Matches\"";
            var dbSource = new DatabaseSource(NpgsqlFactory.Instance, connectionString, sqlCommand);

            // load the data 
            System.Console.WriteLine("Loading training data....");
            var trainingDataView = loader.Load(dbSource);
            System.Console.WriteLine("done");


            // Data process configuration with pipeline data transformations 
            var dataProcessPipeline =
                mlContext.Transforms.Conversion.MapValueToKey("Winner", "Winner")
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(
                    new[] { new InputOutputColumnPair("HomeTeam", "HomeTeam"),
                        new InputOutputColumnPair("AwayTeam", "AwayTeam") }))
                .Append(mlContext.Transforms.Concatenate("Features", new[] { "HomeTeam", "AwayTeam" }))
                .Append(mlContext.Transforms.NormalizeMinMax("Features", "Features"))
                .AppendCacheCheckpoint(mlContext);

            // Set the training algorithm 
            var trainer = mlContext.MulticlassClassification.Trainers.OneVersusAll(mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression(labelColumnName: "Winner", featureColumnName: "Features"), labelColumnName: "Winner")
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));
            var trainingPipeline = dataProcessPipeline.Append(trainer);

            // Cross-Validate with single dataset (since we don't have two datasets, one for training and for evaluate)
            // in order to evaluate and get the model's accuracy metrics
            System.Console.WriteLine("=============== Cross-validating to get model's accuracy metrics ===============");
            var crossValidationResults = mlContext.MulticlassClassification.CrossValidate(trainingDataView, trainingPipeline, numberOfFolds: 5, labelColumnName: "Winner");
            PrintMulticlassClassificationFoldsAverageMetrics(crossValidationResults);

            System.Console.WriteLine("=============== Training  model ===============");
            ITransformer mlModel = trainingPipeline.Fit(trainingDataView);
            System.Console.WriteLine("=============== End of training process ===============");

            // Make predictions
            var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
            ModelOutput predictionResult = predEngine.Predict(new ModelInput
            {
                HomeTeam = "West Ham United FC",
                AwayTeam = "Manchester City FC"
            });

            // Show the prediction
            System.Console.WriteLine($"Single prediction:");
            System.Console.WriteLine($"  Predicted winnder: {predictionResult.Prediction}");
            System.Console.WriteLine($"  Actual winner: Manchester City FC");
            System.Console.WriteLine($"Home Win: {predictionResult.Score[0] * 100:#.##}%");
            System.Console.WriteLine($"Away Win: {predictionResult.Score[1] * 100:#.##}%");
            System.Console.WriteLine($"Draw: {predictionResult.Score[2] * 100:#.##}%");
        }

        public static void PrintMulticlassClassificationFoldsAverageMetrics(IEnumerable<TrainCatalogBase.CrossValidationResult<MulticlassClassificationMetrics>> crossValResults)
        {
            var metricsInMultipleFolds = crossValResults.Select(r => r.Metrics);

            var microAccuracyValues = metricsInMultipleFolds.Select(m => m.MicroAccuracy);
            var microAccuracyAverage = microAccuracyValues.Average();
            var microAccuraciesStdDeviation = CalculateStandardDeviation(microAccuracyValues);
            var microAccuraciesConfidenceInterval95 = CalculateConfidenceInterval95(microAccuracyValues);

            var macroAccuracyValues = metricsInMultipleFolds.Select(m => m.MacroAccuracy);
            var macroAccuracyAverage = macroAccuracyValues.Average();
            var macroAccuraciesStdDeviation = CalculateStandardDeviation(macroAccuracyValues);
            var macroAccuraciesConfidenceInterval95 = CalculateConfidenceInterval95(macroAccuracyValues);

            var logLossValues = metricsInMultipleFolds.Select(m => m.LogLoss);
            var logLossAverage = logLossValues.Average();
            var logLossStdDeviation = CalculateStandardDeviation(logLossValues);
            var logLossConfidenceInterval95 = CalculateConfidenceInterval95(logLossValues);

            var logLossReductionValues = metricsInMultipleFolds.Select(m => m.LogLossReduction);
            var logLossReductionAverage = logLossReductionValues.Average();
            var logLossReductionStdDeviation = CalculateStandardDeviation(logLossReductionValues);
            var logLossReductionConfidenceInterval95 = CalculateConfidenceInterval95(logLossReductionValues);

            System.Console.WriteLine($"*************************************************************************************************************");
            System.Console.WriteLine($"*       Metrics for Multi-class Classification model      ");
            System.Console.WriteLine($"*------------------------------------------------------------------------------------------------------------");
            System.Console.WriteLine($"*       Average MicroAccuracy:    {microAccuracyAverage:0.###}  - Standard deviation: ({microAccuraciesStdDeviation:#.###})  - Confidence Interval 95%: ({microAccuraciesConfidenceInterval95:#.###})");
            System.Console.WriteLine($"*       Average MacroAccuracy:    {macroAccuracyAverage:0.###}  - Standard deviation: ({macroAccuraciesStdDeviation:#.###})  - Confidence Interval 95%: ({macroAccuraciesConfidenceInterval95:#.###})");
            System.Console.WriteLine($"*       Average LogLoss:          {logLossAverage:#.###}  - Standard deviation: ({logLossStdDeviation:#.###})  - Confidence Interval 95%: ({logLossConfidenceInterval95:#.###})");
            System.Console.WriteLine($"*       Average LogLossReduction: {logLossReductionAverage:#.###}  - Standard deviation: ({logLossReductionStdDeviation:#.###})  - Confidence Interval 95%: ({logLossReductionConfidenceInterval95:#.###})");
            System.Console.WriteLine($"*************************************************************************************************************");

        }

        public static double CalculateStandardDeviation(IEnumerable<double> values)
        {
            double average = values.Average();
            double sumOfSquaresOfDifferences = values.Select(val => (val - average) * (val - average)).Sum();
            double standardDeviation = Math.Sqrt(sumOfSquaresOfDifferences / (values.Count() - 1));
            return standardDeviation;
        }

        public static double CalculateConfidenceInterval95(IEnumerable<double> values)
        {
            double confidenceInterval95 = 1.96 * CalculateStandardDeviation(values) / Math.Sqrt((values.Count() - 1));
            return confidenceInterval95;
        }
    }
}
