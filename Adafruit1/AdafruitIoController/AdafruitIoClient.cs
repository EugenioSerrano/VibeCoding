using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;

namespace AdafruitIoController
{
    public class AdafruitIoClient
    {
        private readonly string _username;
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public AdafruitIoClient(string username, string apiKey)
        {
            _username = username;
            _apiKey = apiKey;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-AIO-Key", _apiKey);
        }

        public async Task<List<Feed>> GetFeedsAsync()
        {
            var url = $"https://io.adafruit.com/api/v2/{_username}/feeds";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var feeds = JsonSerializer.Deserialize<List<Feed>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return feeds ?? new List<Feed>();
        }

        public async Task<bool> SendFeedValueAsync(string feedKey, string value)
        {
            var url = $"https://io.adafruit.com/api/v2/{_username}/feeds/{feedKey}/data";
            var content = new StringContent($"{{\"value\": \"{value}\"}}", System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            return response.IsSuccessStatusCode;
        }

        public async Task<string> GetFeedLastValueAsync(string feedKey)
        {
            var url = $"https://io.adafruit.com/api/v2/{_username}/feeds/{feedKey}/data/last";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return "";
            var json = await response.Content.ReadAsStringAsync();
            try
            {
                var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("value", out var valueProp))
                    return valueProp.GetString() ?? "";
            }
            catch { }
            return "";
        }
    }

    public class Feed
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Key { get; set; } = "";
        public string Description { get; set; } = "";
        public string LastValue { get; set; } = "";
    }
}
