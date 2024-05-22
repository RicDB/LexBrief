using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Contracts
{
    public interface ICommentsService
    {
        IList<CommentDto> GetComments();
        CommentDto AddComment(CommentDto comment);
        Task<string> GetCommentsSummary(IEnumerable<CommentDto> comments);
    }
}
