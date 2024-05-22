using BLL.Contracts;
using BLL.DTOs;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LexBrief.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private IDocumentService _documentService;
        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }
        // GET: api/<DoumentController>
        [HttpGet]
        public IEnumerable<DocumentDto> Get()
        {
            return _documentService.GetDocuments();
        }

        // GET api/<DoumentController>/5
        [HttpGet("{id}")]
        public DocumentDetailDto Get(int id)
        {
            return _documentService.GetDocument(id);
        }
    }
}
