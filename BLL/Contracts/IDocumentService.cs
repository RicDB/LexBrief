using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Contracts
{
    public interface IDocumentService
    {
        Task<DocumentDetailDto> GetDocument(int id);
        IList<DocumentDto> GetDocuments();
        Task<string> GetDocumentSummary(int id);
    }
}
