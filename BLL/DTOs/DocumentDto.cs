using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class DocumentDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
    }

    public class DocumentDetailDto : DocumentDto
    {
        public string Content { get; set; }
    }
}
