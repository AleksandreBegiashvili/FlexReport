namespace FlexReport.Application.Common.Interfaces.External;

public interface IOpenAIClient
{
    Task<string> SendMessage(string schema, string prompt);
}
