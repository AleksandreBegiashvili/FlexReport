namespace FlexReport.Application.Integrations.OpenAI;

public interface IOpenAIClient
{
    Task<string> SendMessage(string schema, string prompt);
}
