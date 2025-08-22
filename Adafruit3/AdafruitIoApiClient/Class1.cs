
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace AdafruitIoApiClient;

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

	// MODELOS
	public class Dashboard
	{
		[JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
		[JsonPropertyName("description")] public string? Description { get; set; }
		[JsonPropertyName("key")] public string Key { get; set; } = string.Empty;
	}

	public class Block
	{
		[JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
		[JsonPropertyName("key")] public string Key { get; set; } = string.Empty;
		[JsonPropertyName("visual_type")] public string VisualType { get; set; } = string.Empty;
		[JsonPropertyName("block_feeds")] public List<BlockFeed> BlockFeeds { get; set; } = new();
	}
	public class BlockFeed
	{
		[JsonPropertyName("id")]
		[JsonConverter(typeof(StringOrNumberConverter))]
		public string Id { get; set; } = string.Empty;
		[JsonPropertyName("feed")] public Feed? Feed { get; set; }
	}

	// Conversor para aceptar string o número en JSON
	public class StringOrNumberConverter : System.Text.Json.Serialization.JsonConverter<string>
	{
		public override string Read(ref System.Text.Json.Utf8JsonReader reader, Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
		{
			return reader.TokenType switch
			{
				System.Text.Json.JsonTokenType.String => reader.GetString()!,
				System.Text.Json.JsonTokenType.Number => reader.GetInt64().ToString(),
				_ => throw new System.Text.Json.JsonException($"Unexpected token {reader.TokenType}")
			};
		}
		public override void Write(System.Text.Json.Utf8JsonWriter writer, string value, System.Text.Json.JsonSerializerOptions options)
		{
			writer.WriteStringValue(value);
		}
	}
	public class Feed
	{
		[JsonPropertyName("id")] public int Id { get; set; }
		[JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
		[JsonPropertyName("key")] public string Key { get; set; } = string.Empty;
		[JsonPropertyName("last_value")] public string? LastValue { get; set; }
	}
	public class FeedData
	{
		[JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
		[JsonPropertyName("value")] public string Value { get; set; } = string.Empty;
		[JsonPropertyName("created_at")] public string CreatedAt { get; set; } = string.Empty;
	}

	// MÉTODOS PRINCIPALES

	public async Task<List<Dashboard>> GetDashboardsAsync()
	{
		var url = $"{_baseUrl}/dashboards";
		var dashboards = await _httpClient.GetFromJsonAsync<List<Dashboard>>(url);
		return dashboards ?? new List<Dashboard>();
	}

	public async Task<List<Block>> GetBlocksAsync(string dashboardKey)
	{
		var url = $"{_baseUrl}/dashboards/{dashboardKey}/blocks";
		var blocks = await _httpClient.GetFromJsonAsync<List<Block>>(url);
		return blocks ?? new List<Block>();
	}

	public async Task<List<Feed>> GetFeedsAsync()
	{
		var url = $"{_baseUrl}/feeds";
		var feeds = await _httpClient.GetFromJsonAsync<List<Feed>>(url);
		return feeds ?? new List<Feed>();
	}

	public async Task<FeedData?> GetFeedLastDataAsync(string feedKey)
	{
		var url = $"{_baseUrl}/feeds/{feedKey}/data/last";
		try
		{
			var response = await _httpClient.GetAsync(url);
			if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
				return null;
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadFromJsonAsync<FeedData>();
		}
		catch (HttpRequestException)
		{
			return null;
		}
	}

	public async Task<bool> SendFeedDataAsync(string feedKey, string value)
	{
		var url = $"{_baseUrl}/feeds/{feedKey}/data";
		var content = JsonContent.Create(new { value });
		var response = await _httpClient.PostAsync(url, content);
		return response.IsSuccessStatusCode;
	}
}
