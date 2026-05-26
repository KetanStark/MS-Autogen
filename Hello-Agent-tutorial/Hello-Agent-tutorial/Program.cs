using AutoGen.OpenAI;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.ClientModel.Primitives;
using System.Speech.Recognition;
using System.Speech.Synthesis;

//Open-router key
var apiKey = "Your-OpenRouter-API-KEY";

if (string.IsNullOrEmpty(apiKey))
{
    throw new Exception("OPENROUTER_API_KEY is missing");
}

// OpenRouter endpoint
var endpoint = new Uri("https://openrouter.ai/api/v1");

// HttpClient
var httpClient = new HttpClient();

httpClient.DefaultRequestHeaders.Add("HTTP-Referer", "http://localhost");

httpClient.DefaultRequestHeaders.Add("X-OpenRouter-Title", "AutoGen Console Chat");

// OpenRouter endpoint
var openRouterEndpoint = new Uri("https://openrouter.ai/api/v1");

// Create OpenAI-compatible client
var openAIClient = new OpenAIClient(
        new ApiKeyCredential(apiKey),
        new OpenAIClientOptions
        {
            Endpoint = openRouterEndpoint,
            Transport = new HttpClientPipelineTransport(httpClient)
        }
    );

// OpenRouter model
var model = "openai/gpt-4o-mini";

// Create chat client
var chatClient = openAIClient.GetChatClient(model);

// Create AutoGen agent
var agent = new OpenAIChatAgent(
    chatClient: chatClient,
    name: "assistant",
    systemMessage: "You are a helpful AI assistant."
);

// Conversation history
List<ChatMessage> messages =
[
    new SystemChatMessage("You are a helpful AI assistant.")
];

Console.WriteLine("====================================");
Console.WriteLine(" OpenRouter AutoGen Chat ");
Console.WriteLine(" Type 'exit' to quit ");
Console.WriteLine("====================================");

while (true)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("\nYou: ");
    Console.ResetColor();

    var userInput = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(userInput))
    {
        continue;
    }

    if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    // Voice input mode
    if (userInput.Equals("voice", StringComparison.OrdinalIgnoreCase))
    {
        userInput = GetVoiceInput();

        // Create a new instance of the SpeechSynthesizer
        using (SpeechSynthesizer synth = new SpeechSynthesizer())
        {
            // Configure the voice (optional)
            synth.SelectVoiceByHints(VoiceGender.Neutral);

            // Get user input
            Console.WriteLine("Enter the text you want to convert to speech:");
            string text = Console.ReadLine();

            // Speak the text
            synth.Speak(text); // Synchronously speak the text
        }

        Console.WriteLine($"\nRecognized: {userInput}");

        if (string.IsNullOrWhiteSpace(userInput))
        {
            continue;
        }
    }

    // Add user message
    messages.Add(new UserChatMessage(userInput));

    try
    {
        // Request completion
        ChatCompletion completion = await chatClient.CompleteChatAsync(messages);

        // Get response
        var assistantReply = completion.Content[0].Text;

        Console.ForegroundColor = ConsoleColor.Cyan;
        // Print response
        Console.Write($"\nAssistant: ");
        Console.ResetColor();
        Console.WriteLine($"{assistantReply}");

        // Save assistant response
        messages.Add(new AssistantChatMessage(assistantReply));
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\nError: {ex.Message}");
    }
}

Console.WriteLine("\nChat ended.");


// ===========================
// Voice Recognition Function
// ===========================
static string GetVoiceInput()
{
    try
    {
        using SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();

        recognizer.SetInputToDefaultAudioDevice();

        recognizer.LoadGrammar(new DictationGrammar());

        Console.WriteLine("\nListening... Speak now.");

        RecognitionResult result =
            recognizer.Recognize();

        return result?.Text ?? "";
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\nVoice Error: {ex.Message}");
        return "";
    }
}
