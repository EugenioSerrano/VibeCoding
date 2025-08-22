using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AdafruitIoClient;

public class AdafruitIoApiClient
{
    private readonly HttpClient _httpClient;
    private readonly AdafruitIoOptions _options;
    private readonly ILogger<AdafruitIoApiClient>? _logger;

    public AdafruitIoApiClient(HttpClient httpClient, IOptions<AdafruitIoOptions> options, ILogger<AdafruitIoApiClient>? logger = null)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
        _httpClient.BaseAddress = new Uri(_options.BaseUrl);
        _httpClient.DefaultRequestHeaders.Add("X-AIO-Key", _options.ApiKey);
    }

    // 1. Obtener todos los dashboards
    public async Task<List<DashboardDto>> GetDashboardsAsync(CancellationToken cancellationToken = default)
    {
        var url = $"{_options.Username}/dashboards";
        var dashboards = await _httpClient.GetFromJsonAsync<List<DashboardDto>>(url, cancellationToken);
        return dashboards ?? new();
    }

    // 2. Obtener los bloques de un dashboard
    public async Task<List<BlockDto>> GetBlocksAsync(string dashboardKey, CancellationToken cancellationToken = default)
    {
        var url = $"{_options.Username}/dashboards/{dashboardKey}/blocks";
        var blocks = await _httpClient.GetFromJsonAsync<List<BlockDto>>(url, cancellationToken);
        return blocks ?? new();
    }

    // 3. Obtener todos los feeds (componentes) de un usuario
    public async Task<List<FeedDto>> GetFeedsAsync(CancellationToken cancellationToken = default)
    {
        var url = $"{_options.Username}/feeds";
        var feeds = await _httpClient.GetFromJsonAsync<List<FeedDto>>(url, cancellationToken);
        return feeds ?? new();
    }

    // 4. Obtener el estado actual de un feed
    public async Task<FeedDataDto?> GetFeedLastValueAsync(string feedKey, CancellationToken cancellationToken = default)
    {
        var url = $"{_options.Username}/feeds/{feedKey}/data/last";
        return await _httpClient.GetFromJsonAsync<FeedDataDto>(url, cancellationToken);
    }

    // 5. Modificar el valor de un feed
    public async Task<bool> SetFeedValueAsync(string feedKey, string value, CancellationToken cancellationToken = default)
    {
        var url = $"{_options.Username}/feeds/{feedKey}/data";
        var content = JsonContent.Create(new { value });
        var response = await _httpClient.PostAsync(url, content, cancellationToken);
        return response.IsSuccessStatusCode;
    }
}

// DTOs para deserialización
public class DashboardDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Key { get; set; }
}

public class BlockDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Key { get; set; }
    public string? Visual_Type { get; set; }
    public List<BlockFeedDto>? Block_Feeds { get; set; }
}

public class BlockFeedDto
{
    public string? Id { get; set; }
    public FeedDto? Feed { get; set; }
}

public class FeedDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Key { get; set; }
    public string? Description { get; set; }
    public string? Last_Value { get; set; }
}

public class FeedDataDto
{
    public string? Id { get; set; }
    public string? Value { get; set; }
    public string? Created_At { get; set; }
}
