using Azure;
using Azure.AI.OpenAI;

namespace BLL.Helpers
{
    public static class Helpers
    {
        public static async Task<string> ExtractSummary(string prompt, string[] questions)
        {
            string summury = string.Empty;

            try
            {
                string key = "1e0628f09d2b4a4dab569af8613c155d";

                string endpoint = "https://chat-fast-25bbd9bb.openai.azure.com/";

                string engine = "gpt-35-turbo-0613";

                // Configure OpenAI API client

                OpenAIClient client = new(new Uri(endpoint), new AzureKeyCredential(key));

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
