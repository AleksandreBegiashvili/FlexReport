namespace FlexReport.Infrastructure.Configuration;

public class OpenAIConfiguration
{
    public string ApiKey { get; set; } = string.Empty;
    public string ChatGptModel { get; set; } = string.Empty;
    public int MaxTokens { get; set; }
    public int FrequencyPenalty { get; set; }
    public double Temperature { get; set; }
}
