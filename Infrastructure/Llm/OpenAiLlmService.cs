using Application.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;

namespace Infrastructure.Llm;

public class OpenAiLlmService(string apiKey) : ILlmService
{
    private readonly HttpClient _http = new();

    public async Task<Response> GenerateResponseAsync(Ticket ticket)
    {
        var prompt = $"Respond to this ticket:\n\n\"{ticket.CustomerMessage}\"";

        var requestBody = new
        {
            model = "gpt-4o-mini",
            messages = new[] {
                new { role = "user", content = prompt }
            }
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
        request.Headers.Add("Authorization", $"Bearer {apiKey}");
        request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(requestBody));
        request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = System.Text.Json.JsonDocument.Parse(json);

        var content = result.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return new Response(content ?? "[LLM returned empty]");
    }
}
