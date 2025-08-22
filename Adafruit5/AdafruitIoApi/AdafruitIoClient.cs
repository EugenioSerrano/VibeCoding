using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AdafruitIoApi
{
    public class AdafruitIoClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _username;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public AdafruitIoClient(string username, string apiKey)
        {
            _username = username;
            _apiKey = apiKey;
            _baseUrl = $"https://io.adafruit.com/api/v2/{_username}";
            _httpClient = new HttpClient();
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

        // Obtiene todos los bloques (componentes) de un dashboard
        public async Task<List<Block>> GetDashboardBlocksAsync(string dashboardKey)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/dashboards/{dashboardKey}/blocks");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Block>>(json) ?? new();
        }

        // Obtiene los feeds de un dashboard (por cada bloque, obtiene los feeds asociados)
        public async Task<List<Feed>> GetFeedsFromDashboardAsync(string dashboardKey)
        {
            var blocks = await GetDashboardBlocksAsync(dashboardKey);
            var feeds = new List<Feed>();
            foreach (var block in blocks)
            {
                if (block.block_feeds != null)
                {
                    foreach (var bf in block.block_feeds)
                    {
                        if (!string.IsNullOrEmpty(bf.feed?.key))
                        {
                            var feed = await GetFeedAsync(bf.feed.key);
                            if (feed != null)
                                feeds.Add(feed);
                        }
                    }
                }
            }
            return feeds;
        }

        // Obtiene un feed por su key
        public async Task<Feed?> GetFeedAsync(string feedKey)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/feeds/{feedKey}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Feed>(json);
        }

        // Obtiene el valor actual de un feed
        public async Task<string?> GetFeedCurrentValueAsync(string feedKey)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/feeds/{feedKey}/data/last");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<FeedData>(json);
            return data?.value;
        }

        // Modifica el valor de un feed (crea un nuevo dato)
        public async Task<bool> SetFeedValueAsync(string feedKey, string value)
        {
            var content = new StringContent($"{{\"value\":\"{value}\"}}", Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}/feeds/{feedKey}/data", content);
            return response.IsSuccessStatusCode;
        }
    }

    // Modelos para deserialización
    public class Dashboard
    {
        public string? name { get; set; }
        public string? description { get; set; }
        public string? key { get; set; }
    }

    public class Block
    {
        public string? name { get; set; }
        public string? key { get; set; }
        public List<BlockFeed>? block_feeds { get; set; }
    }

    public class BlockFeed
    {
        public string? id { get; set; }
        public FeedRef? feed { get; set; }
        public object? group { get; set; }
    }

    public class FeedRef
    {
        public string? key { get; set; }
    }

    public class Feed
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? key { get; set; }
        public string? last_value { get; set; }
        public string? status { get; set; }
    }

    public class FeedData
    {
        public string? id { get; set; }
        public string? value { get; set; }
        public int feed_id { get; set; }
        public string? feed_key { get; set; }
        public string? created_at { get; set; }
    }
}
