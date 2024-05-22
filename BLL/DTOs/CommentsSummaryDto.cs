using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class CommentsSummaryDto
    {
        public string Content { get; set; }
        public string Positive { get; set; }
        public string Neutral { get; set; }
        public string Negative { get; set; }
    }
}
