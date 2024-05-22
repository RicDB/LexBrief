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
            bool isPositiveCase = new Random().Next(0, 2) > 0;

            int posMin = isPositiveCase ? 3 : 1;
            int posMax = isPositiveCase ? 10 : 3;

            int negMin = !isPositiveCase ? 3 : 1;
            int negMax = !isPositiveCase ? 10 : 3;

            var positiveCommentList = commentsList.Where(it=>it.Sentiment > 0.5f).OrderBy(x => new Random().Next()).Take(new Random().Next(posMin, posMax)).ToList();
            var negativeCommentList = commentsList.Where(it => it.Sentiment < 0.5f).OrderBy(x => new Random().Next()).Take(new Random().Next(negMin, negMax)).ToList();
            return positiveCommentList.Concat(negativeCommentList).OrderBy(x => new Random().Next()).ToList();
        }

        public async Task<CommentsSummaryDto> GetCommentsSummary(IEnumerable<CommentDto> comments)
        {
            string commentsSummary = string.Empty;

            var listComments = comments.ToList();
            string listCommentsString = string.Empty;

            listComments.ForEach(it => listCommentsString += $"- {it.Content};{Environment.NewLine}");

            commentsSummary = await ExtractSummaryFromComments(listCommentsString);

            return new CommentsSummaryDto() { Content = commentsSummary };
        }

        private async Task<string> ExtractSummaryFromComments(string stringComments)
        {
            string summury = string.Empty;

            try
            {
                string prompt = $"This is a list of comments \n\n{stringComments}\n\n";

                // Ask questions and get answers

                string[] questions = {
                    "Could you create a summary without bullet point in one sentence in max 25 words?",
                };

                summury = await Helpers.Helpers.ExtractSummary(prompt, questions);
            }
            catch (Exception ex)
            {
                summury = $"Error creating summary - ex: {ex.Message}";
            }

            return summury;
        }
    }
}
