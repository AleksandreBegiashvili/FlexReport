using FlexReport.Application.Integrations.OpenAI;
using FlexReport.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using OpenAI_API;
using OpenAI_API.Chat;

namespace FlexReport.Infrastructure.Integrations.OpenAI;

public class OpenAIClient : IOpenAIClient
{
    private readonly OpenAIConfiguration _openAIClientConfiguration;

    public OpenAIClient(IOptions<OpenAIConfiguration> options)
    {
        _openAIClientConfiguration = options.Value;
    }

    private const string _systemMessage = "You are an AI assistant tasked to generate SQL queries given database schema and prompt.";
    private const string _explanation = "Let's think step by step:" +
        "1. Analyze the provided database schema;" +
        "2. Correctly use column names and pay attention to junction tables for many-to-many relationships;" +
        "3. Generate the correct SQL query based on the prompt, think twice;";

    public async Task<string> SendMessage(string schema, string prompt)
    {
        var authentication = new APIAuthentication(_openAIClientConfiguration.ApiKey);
        var api = new OpenAIAPI(authentication);

        var request = BuildChatRequest(schema, prompt);
        var result = await api.Chat.CreateChatCompletionAsync(request);

        return ExtractQuery(result);
    }

    private ChatRequest BuildChatRequest(string schema, string prompt)
        => new()
        {
            Model = _openAIClientConfiguration.ChatGptModel,
            MaxTokens = _openAIClientConfiguration.MaxTokens,
            FrequencyPenalty = _openAIClientConfiguration.FrequencyPenalty,
            Temperature = _openAIClientConfiguration.Temperature,
            Messages = new List<ChatMessage>()
            {
                new ChatMessage(ChatMessageRole.System, _systemMessage),
                new ChatMessage(ChatMessageRole.User, _explanation),
                new ChatMessage(ChatMessageRole.User, $"Here is the schema: {schema}"),
                new ChatMessage(ChatMessageRole.User, $"Write the SQL query based on above schema for the following prompt: {prompt}")
            }
        };

    private static string ExtractQuery(ChatResult result)
    {
        const string startSeparator = "```sql";
        const string endSeparator = "```";

        var message = result.Choices[result.Choices.Count - 1]?.Message?.Content;

        if (string.IsNullOrWhiteSpace(message))
        {
            throw new Exception("Failed to receive response from ChatGPT");
        }

        var startIndex = message.IndexOf(startSeparator);
        var endIndex = message.LastIndexOf(endSeparator);

        var query = message[(startIndex + startSeparator.Length)..endIndex];

        return query;
    }
}
