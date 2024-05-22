using BLL.Contracts;
using BLL.DTOs;
using Newtonsoft.Json;
using System.Reflection;

namespace BLL.Models
{
    public class CommentService : ICommentsService
    {
        private IEnumerable<CommentDto> commentsList;
        private BLL.SentimentAnalysis.SentimentAnalysis sentimentAnalysis;

        List<string> ownerList = new List<string>()
            {
                "DR",
                "LM",
                "DF",
                "FG",
                "AM",
                "PA"
            };

        public CommentService() 
        {
            sentimentAnalysis = new SentimentAnalysis.SentimentAnalysis();

            string jsonPath = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "Data", "Comments");
            
            if (Directory.Exists(jsonPath))
            {
                var data = File.ReadAllText(Path.Combine(jsonPath, "comments.json"));

                List<SentimentAnalysis.SentimentPrediction> desData = JsonConvert.DeserializeObject<List<SentimentAnalysis.SentimentPrediction>>(data);

                commentsList = desData.Select(x => new CommentDto { Content = x.SentimentText, Sentiment = x.Score, OwnerInitials = ownerList[new Random().Next(0, ownerList.Count - 1)] });
            }

        }

        public CommentDto AddComment(CommentDto comment)
        {
            var sentimentPrediction = sentimentAnalysis.Analysis(comment.Content);
            comment.Sentiment = sentimentPrediction.Probability;
            return comment;
        }

        public IList<CommentDto> GetComments()
        {          
            return commentsList.OrderBy(x => new Random().Next()).Take(new Random().Next(5, 20)).ToList();
        }
    }
}
