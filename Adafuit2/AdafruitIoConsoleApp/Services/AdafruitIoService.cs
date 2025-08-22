using System.Net.Http.Headers;
using System.Text.Json;
using AdafruitIoConsoleApp.Models;

namespace AdafruitIoConsoleApp.Services
{
    public class AdafruitIoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _username;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public AdafruitIoService(string username, string apiKey)
        {
            _username = username;
            _apiKey = apiKey;
            _baseUrl = $"https://io.adafruit.com/api/v2/{_username}";
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-AIO-Key", _apiKey);
        }

        public async Task<List<Feed>> GetFeedsAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/feeds");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Feed>>(json) ?? new List<Feed>();
        }

        public async Task<Feed?> GetFeedAsync(string feedKey)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/feeds/{feedKey}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Feed>(json);
        }

        public async Task<List<FeedData>> GetFeedDataAsync(string feedKey, int limit = 1)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/feeds/{feedKey}/data?limit={limit}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<FeedData>>(json) ?? new List<FeedData>();
        }

        public async Task<bool> SendFeedDataAsync(string feedKey, string value)
        {
            var content = new StringContent($"{{\"value\":\"{value}\"}}", System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}/feeds/{feedKey}/data", content);
            return response.IsSuccessStatusCode;
        }
    }
}
