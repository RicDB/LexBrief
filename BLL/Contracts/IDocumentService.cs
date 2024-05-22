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
        DocumentDetailDto GetDocument(int id);
        IList<DocumentDto> GetDocuments();
    }
}
