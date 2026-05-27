using OpenAI;
using OpenAI.Chat;

using System.ClientModel;
using System.ClientModel.Primitives;

namespace OpenRouterWebChat.Services;

public class OpenRouterService
{
    private readonly ChatClient _chatClient;
    private readonly IConfiguration _configuration;
    

    private readonly List<ChatMessage> _messages =
    [
        new SystemChatMessage("""
        You are a helpful AI assistant.
        Keep answers concise and clear.
        """)
    ];

    public OpenRouterService(IConfiguration configuration)
    {
        _configuration = configuration;

        var apiKey = _configuration["OpenRouter:ApiKey"];

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new Exception(
                "OPENROUTER_API_KEY not found");
        }

        var endpoint =
            new Uri("https://openrouter.ai/api/v1");

        var httpClient = new HttpClient();

        httpClient.DefaultRequestHeaders.Add(
            "HTTP-Referer",
            "http://localhost");

        httpClient.DefaultRequestHeaders.Add(
            "X-OpenRouter-Title",
            "ASP.NET OpenRouter Chat");

        var client = new OpenAIClient(
            new ApiKeyCredential(apiKey),
            new OpenAIClientOptions
            {
                Endpoint = endpoint,
                Transport =
                    new HttpClientPipelineTransport(
                        httpClient)
            });

        var model = "openai/gpt-4o-mini";

        _chatClient = client.GetChatClient(model);
    }

    public async Task<string> AskAsync(string message)
    {
        _messages.Add(new UserChatMessage(message));

        ChatCompletion completion =
            await _chatClient.CompleteChatAsync(_messages);

        var reply =
            completion.Content[0].Text;

        _messages.Add(
            new AssistantChatMessage(reply));

        return reply;
    }
}