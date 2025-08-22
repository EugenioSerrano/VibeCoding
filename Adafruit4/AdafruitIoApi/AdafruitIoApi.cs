using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace AdafruitIoApi
{
    public class AdafruitIoClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _username;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public AdafruitIoClient(string username, string apiKey, HttpClient? httpClient = null)
        {
            _username = username;
            _apiKey = apiKey;
            _baseUrl = $"https://io.adafruit.com/api/v2/{_username}";
            _httpClient = httpClient ?? new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-AIO-Key", _apiKey);
        }

        // Obtiene todos los dashboards
        public async Task<List<Dashboard>> GetDashboardsAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/dashboards");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Dashboard>>(json) ?? new();
        }

        // Obtiene todos los feeds de un dashboard (por dashboardKey)
        public async Task<List<Feed>> GetFeedsFromDashboardAsync(string dashboardKey)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/dashboards/{dashboardKey}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var dashboard = JsonSerializer.Deserialize<Dashboard>(json);
            return dashboard?.Blocks
                .SelectMany(b => b.BlockFeeds.Select(f => f.Feed))
                .Where(f => f != null)
                .ToList() ?? new();
        }

        // Obtiene el estado actual (último valor) de un feed
        public async Task<FeedData?> GetFeedLastValueAsync(string feedKey)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/feeds/{feedKey}/data/last");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<FeedData>(json);
        }

        // Modifica el valor de un feed (crea nuevo dato)
        public async Task<FeedData?> SetFeedValueAsync(string feedKey, string value)
        {
            var content = new StringContent($"{{\"value\":\"{value}\"}}", Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}/feeds/{feedKey}/data", content);
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<FeedData>(json);
        }
    }

    // Modelos para deserialización
    public class Dashboard
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Key { get; set; }
        public List<Block> Blocks { get; set; } = new();
    }
    public class Block
    {
        public string? Name { get; set; }
        public string? Key { get; set; }
        public List<BlockFeed> BlockFeeds { get; set; } = new();
    }
    public class BlockFeed
    {
        public Feed? Feed { get; set; }
    }
    public class Feed
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Key { get; set; }
        public string? LastValue { get; set; }
        public string? Status { get; set; }
    }
    public class FeedData
    {
        public string? Id { get; set; }
        public string? Value { get; set; }
        public string? CreatedAt { get; set; }
    }
}
