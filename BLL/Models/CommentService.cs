using BLL.Contracts;
using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class CommentService : ICommentsService
    {
        private string jsonPath;
        public CommentService() {
            jsonPath = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "Data", "Comments");
        }

        public CommentDto AddComment(CommentDto comment)
        {
            return new CommentDto();
        }

        public IList<CommentDto> GetComments()
        {
            IEnumerable<CommentDto> res = new List<CommentDto>();
            var ownerList = new List<string>()
            {
                "DR",
                "LM",
                "DF",
                "FG",
                "AM",
                "PA"
            };
            if (Directory.Exists(jsonPath))
            {
                var data = File.ReadAllText(Path.Combine(jsonPath, "comments.json"));

                var desData = JsonSerializer.Deserialize<IList<Class1>>(data);

                res = desData.Select(x => new CommentDto { Content = x.SentimentText, Sentment = x.Score, OwnerInitials = ownerList[new Random().Next(0, ownerList.Count - 1)] });
            }

            

            return res.OrderBy(x => new Random().Next()).Take(new Random().Next(5, 20)).ToList();
        }
    }
}
