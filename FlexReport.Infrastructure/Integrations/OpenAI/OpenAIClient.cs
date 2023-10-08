using FlexReport.Application.Integrations.OpenAI;
using FlexReport.Domain.Exceptions;
using FlexReport.Infrastructure.Configuration;
using FlexReport.Infrastructure.Constants;
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
                new ChatMessage(ChatMessageRole.System, OpenAIConstants.SystemMessage),
                new ChatMessage(ChatMessageRole.User, OpenAIConstants.Explanation),
                new ChatMessage(ChatMessageRole.User, $"Here is the schema: {schema}"),
                new ChatMessage(ChatMessageRole.User, $"Write the SQL query based on above schema for the following prompt: {prompt}")
            }
        };

    private static string ExtractQuery(ChatResult result)
    {
        var message = result.Choices[result.Choices.Count - 1]?.Message?.Content;

        if (string.IsNullOrWhiteSpace(message))
        {
            throw new InvalidChatGptResponseException();
        }

        var startIndex = message.IndexOf(OpenAIConstants.SqlStartSeparator);
        var endIndex = message.LastIndexOf(OpenAIConstants.SqlEndSeparator);

        var query = message[(startIndex + OpenAIConstants.SqlStartSeparator.Length)..endIndex];

        return query;
    }
}
