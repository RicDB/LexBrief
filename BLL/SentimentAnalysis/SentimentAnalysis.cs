using Microsoft.ML;
using Microsoft.ML.Data;
using static Microsoft.ML.DataOperationsCatalog;

namespace BLL.SentimentAnalysis
{
    public class SentimentAnalysis
    {
        private MLContext mlContext;
        private ITransformer model;

        string _dataPathTrainingSet = Path.Join(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Data", "yelp-amazon_cells-imdb_70.txt");
        string _dataPathTestSet = Path.Join(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Data", "testSet.txt");

        public SentimentAnalysis(string dataPathTrainingSet = null, string dataPathTestSet = null, bool training = false)
        {
            if(dataPathTrainingSet != null)
                _dataPathTrainingSet = dataPathTrainingSet;

            if (dataPathTestSet != null)
                _dataPathTestSet = dataPathTestSet;

            mlContext = new MLContext();
            Init(training);
        }

        public void Init(bool training = false)
        {
            if(training)
            {
                TrainTestData splitDataView = LoadData(0.01);

                model = BuildAndTrainModel(splitDataView.TrainSet);

                MLContext mlContextTestSet = new MLContext();


                IDataView dataView = mlContextTestSet.Data.LoadFromTextFile<SentimentData>(_dataPathTestSet, hasHeader: false);

                // You need both a training dataset to train the model and a test dataset to evaluate the model.
                // Split the loaded dataset into train and test datasets
                // Specify test dataset percentage with the `testFraction`parameter
                TrainTestData splitDataViewTestSet = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);


                Evaluate(splitDataViewTestSet.TestSet);

                // Save
                mlContext.Model.Save(model, splitDataView.TrainSet.Schema, "model.zip");
            }
            else 
            {
                //Define DataViewSchema for data preparation pipeline and trained model
                DataViewSchema modelSchema;

                string modelPath = Path.Join(Directory.GetParent(System.Reflection.Assembly.GetEntryAssembly().Location).FullName, "SentimentAnalysis", "model.zip");
                // Load trained model
                model = mlContext.Model.Load(modelPath, out modelSchema);
            }
        }

        public SentimentPrediction Analysis(string comment)
        {
            return UseModelWithSingleItem(comment);
        }

        public List<SentimentPrediction> Analysis(IList<string> comments)
        {
            return UseModelWithBatchItems(comments);
        }

        #region Private
        private TrainTestData LoadData(double testFraction = 0.2)
        {
            // Note that this case, loading your training data from a file,
            // is the easiest way to get started, but ML.NET also allows you
            // to load data from databases or in-memory collections.
            IDataView dataView = mlContext.Data.LoadFromTextFile<SentimentData>(_dataPathTrainingSet, hasHeader: false);

            // You need both a training dataset to train the model and a test dataset to evaluate the model.
            // Split the loaded dataset into train and test datasets
            // Specify test dataset percentage with the `testFraction`parameter
            TrainTestData splitDataView = mlContext.Data.TrainTestSplit(dataView, testFraction: testFraction);

            return splitDataView;
        }

        private ITransformer BuildAndTrainModel(IDataView splitTrainSet)
        {
            // Create a flexible pipeline (composed by a chain of estimators) for creating/training the model.
            // This is used to format and clean the data.
            // Convert the text column to numeric vectors (Features column)
            var estimator = mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Features", inputColumnName: nameof(SentimentData.SentimentText))
            // append the machine learning task to the estimator
            .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));

            // Create and train the model based on the dataset that has been loaded, transformed.
            Console.WriteLine("=============== Create and Train the Model ===============");
            var model = estimator.Fit(splitTrainSet);
            Console.WriteLine("=============== End of training ===============");
            Console.WriteLine();

            // Returns the model we trained to use for evaluation.
            return model;
        }

        private void Evaluate(IDataView splitTestSet)
        {
            // Evaluate the model and show accuracy stats

            //Take the data in, make transformations, output the data.
            Console.WriteLine("=============== Evaluating Model accuracy with Test data===============");
            IDataView predictions = model.Transform(splitTestSet);

            // BinaryClassificationContext.Evaluate returns a BinaryClassificationEvaluator.CalibratedResult
            // that contains the computed overall metrics.
            CalibratedBinaryClassificationMetrics metrics = mlContext.BinaryClassification.Evaluate(predictions, "Label");

            // The Accuracy metric gets the accuracy of a model, which is the proportion
            // of correct predictions in the test set.

            // The AreaUnderROCCurve metric is equal to the probability that the algorithm ranks
            // a randomly chosen positive instance higher than a randomly chosen negative one
            // (assuming 'positive' ranks higher than 'negative').

            // The F1Score metric gets the model's F1 score.
            // The F1 score is the harmonic mean of precision and recall:
            //  2 * precision * recall / (precision + recall).

            Console.WriteLine();
            Console.WriteLine("Model quality metrics evaluation");
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
            Console.WriteLine($"Auc: {metrics.AreaUnderRocCurve:P2}");
            Console.WriteLine($"F1Score: {metrics.F1Score:P2}");
            Console.WriteLine("=============== End of model evaluation ===============");
        }

        private SentimentPrediction UseModelWithSingleItem(string comment = null)
        {
            PredictionEngine<SentimentData, SentimentPrediction> predictionFunction = mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);

            SentimentData sampleStatement = new SentimentData
            {
                SentimentText = comment ?? "This was a very bad steak"
            };

            var resultPrediction = predictionFunction.Predict(sampleStatement);

            //Console.WriteLine();
            //Console.WriteLine("=============== Prediction Test of model with a single sample and test dataset ===============");

            //Console.WriteLine();
            //Console.WriteLine($"Sentiment: {resultPrediction.SentimentText} | Prediction: {(Convert.ToBoolean(resultPrediction.Prediction) ? "Positive" : "Negative")} | Probability: {resultPrediction.Probability} ");

            //Console.WriteLine("=============== End of Predictions ===============");
            //Console.WriteLine();

            return resultPrediction;
        }

        private List<SentimentPrediction> UseModelWithBatchItems(IList<string> comments = null)
        {

            if (comments == null) comments = new List<string>() { "This was a horrible meal", "I love this spaghetti." };

            List<SentimentData> sentiments = new List<SentimentData>();

            foreach (var comment in comments)
            {
                sentiments.Add(new SentimentData { SentimentText = comment });
            }

            // Load batch comments just created
            IDataView batchComments = mlContext.Data.LoadFromEnumerable(sentiments);

            IDataView predictions = model.Transform(batchComments);

            // Use model to predict whether comment data is Positive (1) or Negative (0).
            IEnumerable<SentimentPrediction> predictedResults = mlContext.Data.CreateEnumerable<SentimentPrediction>(predictions, reuseRowObject: false);

            //Console.WriteLine();

            //Console.WriteLine("=============== Prediction Test of loaded model with multiple samples ===============");

            //Console.WriteLine();

            //foreach (SentimentPrediction prediction in predictedResults)
            //{
            //    Console.WriteLine($"Sentiment: {prediction.SentimentText} | Prediction: {(Convert.ToBoolean(prediction.Prediction) ? "Positive" : "Negative")} | Probability: {prediction.Probability} ");
            //}
            //Console.WriteLine("=============== End of predictions ===============");

            return predictedResults.ToList();
        }
        #endregion
    }
}
