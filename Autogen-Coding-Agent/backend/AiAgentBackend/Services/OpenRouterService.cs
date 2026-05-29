using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

public class OpenRouterService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public OpenRouterService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }

    public async Task<string> Ask(string prompt)
    {
        var apiKey = _config["OpenRouter:ApiKey"];
        var body = new
        {
            model = "deepseek/deepseek-chat",
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        var req = new HttpRequestMessage(HttpMethod.Post, "https://openrouter.ai/api/v1/chat/completions");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        req.Headers.Add("HTTP-Referer", "http://localhost:3000");
        req.Headers.Add("X-Title", "AI Coding Agent");

        req.Content = new StringContent(
            JsonConvert.SerializeObject(body),
            Encoding.UTF8,
            "application/json");

        var response = await _http.SendAsync(req);
        var responseText = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(responseText);

        dynamic result = JsonConvert.DeserializeObject(responseText);
        return result.choices[0].message.content;
    }
}