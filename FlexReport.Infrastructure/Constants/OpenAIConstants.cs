namespace FlexReport.Infrastructure.Constants;

public static class OpenAIConstants
{
    public const string SystemMessage =
        "You are an AI assistant tasked to generate SQL queries given database schema and prompt.";
    public const string Explanation =
        "Let's think step by step:" +
        "1. Analyze the provided database schema;" +
        "2. Correctly use column names and pay attention to junction tables for many-to-many relationships;" +
        "3. Generate the correct SQL query based on the prompt, think twice;";
    public const string SqlStartSeparator = "```sql";
    public const string SqlEndSeparator = "```";
}
