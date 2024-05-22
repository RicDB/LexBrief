using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class CommentDto
    {
        public string Content { get; set; }
        public float Sentiment { get; set; }
        public string OwnerInitials { get; set; }
    }
}
