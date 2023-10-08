namespace FlexReport.Domain.Exceptions;

public class InvalidChatGptResponseException : Exception
{
    public InvalidChatGptResponseException()
    {
    }

    public InvalidChatGptResponseException(string message)
        : base(message)
    {
    }

    public InvalidChatGptResponseException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
