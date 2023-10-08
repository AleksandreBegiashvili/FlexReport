using AutoFixture;
using FlexReport.Domain.Exceptions;
using FlexReport.Infrastructure.Configuration;
using FlexReport.Infrastructure.Integrations.OpenAI;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using OpenAI_API;
using OpenAI_API.Chat;

namespace FlexReport.Infrastructure.Tests;

public class OpenAIClientTests
{
    private readonly OpenAIClient _openAIClient;
    private readonly Mock<IOpenAIAPI> _openAIApiMock = new();
    private readonly Mock<IChatEndpoint> _chatEndpointMock = new();
    private readonly Fixture _fixture;

    public OpenAIClientTests()
    {
        _fixture = new Fixture();
        var options = Options.Create(new OpenAIConfiguration());
        _openAIClient = new OpenAIClient(_openAIApiMock.Object, options);
    }

    [Fact]
    public async Task SendMessage_GivenValidSchemaAndPrompt_ShouldReturnQuery()
    {
        // Arrange
        var schema = _fixture.Create<string>();
        var prompt = _fixture.Create<string>();
        var validResponse = BuildValidResponse();

        _chatEndpointMock
            .Setup(chat => chat.CreateChatCompletionAsync(It.IsAny<ChatRequest>()))
            .ReturnsAsync(validResponse);
        _openAIApiMock
            .Setup(api => api.Chat)
            .Returns(_chatEndpointMock.Object);

        // Act
        var result = await _openAIClient.SendMessage(schema, prompt);

        // Assert
        result.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task SendMessage_GivenInvalidChatGptResponse_ShouldThrowException()
    {
        // Arrange
        var schema = _fixture.Create<string>();
        var prompt = _fixture.Create<string>();
        var validResponse = BuildInvalidResponse();

        _chatEndpointMock
            .Setup(chat => chat.CreateChatCompletionAsync(It.IsAny<ChatRequest>()))
            .ReturnsAsync(validResponse);
        _openAIApiMock
            .Setup(api => api.Chat)
            .Returns(_chatEndpointMock.Object);

        // Act
        var result = async () => await _openAIClient.SendMessage(schema, prompt);

        // Assert
        await result.Should().ThrowAsync<InvalidChatGptResponseException>();
    }

    private static ChatResult BuildValidResponse()
        => BuildResponse("Here is the generated query: ```sql SELECT * FROM SomeTable ```");

    private static ChatResult BuildInvalidResponse()
        => BuildResponse("Here is the generated query: wrong separator ``sql SELECT * FROM SomeTable ````");

    private static ChatResult BuildResponse(string content)
        => new()
        {
            Choices = new List<ChatChoice>
            {
                new ChatChoice
                {
                    Message = new ChatMessage
                    {
                        Content = content
                    }
                }
            }
        };
}