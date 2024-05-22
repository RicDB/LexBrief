using BLL.Contracts;
using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class DocumentService : IDocumentService
    {
        private string docsPath;
        public DocumentService()
        {
            docsPath = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "Data", "Docs");
        }
        public DocumentDetailDto GetDocument(int id)
        {
            var filePath = Directory.GetFiles(docsPath)[id];
            var file = new DocumentDetailDto()
            {
                Id = id,
                Title = Path.GetFileName(filePath),
                Content = File.ReadAllText(filePath)
            };

            return file;
        }

        public IList<DocumentDto> GetDocuments()
        {

            var res = new List<DocumentDto>();

            if(Directory.Exists(docsPath))
            {
                var docs = Directory.GetFiles(docsPath);

                int id = 0;
                foreach(var doc in docs) {

                    res.Add(new DocumentDto()
                    {
                        Id = id++,
                        Title = Path.GetFileName(doc)
                    });
                }
            }


            return res;
        }
    }
}
