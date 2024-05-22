using Azure;
using Azure.AI.OpenAI;
using BLL.Contracts;
using BLL.DTOs;
using System.Buffers.Text;
using System.Reflection;
using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;


namespace BLL.Models
{
    public class DocumentService : IDocumentService
    {
        private string docsPath;
        public DocumentService()
        {
            docsPath = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "Data", "Docs");
        }

        public async Task<DocumentDetailDto> GetDocument(int id)
        {
            var filePath = Directory.GetFiles(docsPath)[id];
            var file = new DocumentDetailDto()
            {
                Id = id,
                Title = Path.GetFileName(filePath),
                Content = Convert.ToBase64String(File.ReadAllBytes(filePath)),
                Summary = await ExtractSummaryFromDocument(filePath)
            };

            return file;
        }

        public IList<DocumentDto> GetDocuments()
        {

            var res = new List<DocumentDto>();

            if (Directory.Exists(docsPath))
            {
                var docs = Directory.GetFiles(docsPath);

                int id = 0;
                foreach (var doc in docs)
                {

                    res.Add(new DocumentDto()
                    {
                        Id = id++,
                        Title = Path.GetFileName(doc)
                    });
                }
            }


            return res;
        }

        public async Task<string> GetDocumentSummary(int id)
        {
            string summury = string.Empty;

            var filePath = Directory.GetFiles(docsPath)[id];

            return await ExtractSummaryFromDocument(filePath);
        }

        private async Task<string> ExtractSummaryFromDocument(string filePath)
        {
            string summury = string.Empty;

            try
            {
                string key = "1e0628f09d2b4a4dab569af8613c155d";

                string endpoint = "https://chat-fast-25bbd9bb.openai.azure.com/";

                string engine = "gpt-35-turbo-0613";

                // Configure OpenAI API client

                OpenAIClient client = new(new Uri(endpoint), new AzureKeyCredential(key));

                // Load PDF file and extract text

                StringBuilder fullText = new StringBuilder();

                using (PdfDocument pdf = PdfDocument.Open(filePath))
                {
                    for (int i = 0; i < pdf.NumberOfPages; i++)
                    {
                        Page page = pdf.GetPage(i + 1);

                        fullText.Append(page.Text);
                    }
                }

                string prompt = $"What is the answer to the following question regarding the PDF document?\n\n{fullText}\n\n";

                // Ask questions and get answers

                string[] questions = {
                    "What is the document about? I want a summary of up to 10 words. Do not include the number of words used in the summary"
                };

                foreach (string question in questions)
                {
                    string fullPrompt = prompt + question;

                    ChatCompletionsOptions completionsOptions = new ChatCompletionsOptions()
                    {
                        Messages = { new ChatRequestUserMessage(fullPrompt) },
                        DeploymentName = engine
                    };

                    Response<ChatCompletions> response = client.GetChatCompletions(completionsOptions);

                    ChatResponseMessage responseMessage = response.Value.Choices[0].Message;

                    summury = responseMessage.Content;
                }
            }
            catch (Exception ex)
            {
                summury = $"Error creating document summary - ex: {ex.Message}";
            }

            return summury;
        }
    }
}
