using ETicaret_Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ETicaret_Infrastructure.Services
{
    public class OpenAiClient : ILLMClient
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;
        private readonly string _model;

        public OpenAiClient(HttpClient http, IConfiguration configuration)
        {
            _http = http;
            _apiKey = configuration["OpenAI:ApiKey"] ?? "";
            _model = configuration["OpenAI:Model"] ?? "gpt-4o-mini"; // tercihe göre
        }

        public async Task<string> GenerateTextAsync(string prompt, int maxTokens = 150)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
                throw new InvalidOperationException("OpenAI api key is not configured.");

            var requestBody = new
            {
                model = _model,
                messages = new[] {
                    new { role = "user", content = prompt }
                },
                max_tokens = maxTokens,
                temperature = 0.7
            };

            using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            req.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var resp = await _http.SendAsync(req);
            resp.EnsureSuccessStatusCode();

            using var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync());
            // OpenAI response parsing (choices[0].message.content)
            var content = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return content ?? "";
        }
    }
}



