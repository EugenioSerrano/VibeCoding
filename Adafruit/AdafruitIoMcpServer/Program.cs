
using AdafruitIoClient;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Server;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

// Load AdafruitIo options from config
builder.Services.Configure<AdafruitIoOptions>(builder.Configuration.GetSection("AdafruitIo"));
builder.Services.AddHttpClient<AdafruitIoApiClient>();
builder.Services.AddSingleton<AdafruitIoApiClient>(sp =>
{
	var options = sp.GetRequiredService<IOptions<AdafruitIoOptions>>();
	var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(AdafruitIoApiClient));
	var logger = sp.GetService<Microsoft.Extensions.Logging.ILogger<AdafruitIoApiClient>>();
	return new AdafruitIoApiClient(httpClient, options, logger);
});

// Register MCP server and tools
builder.Services.AddMcpServer()
	.WithHttpTransport()
	.WithToolsFromAssembly();


var app = builder.Build();

// Cambia el endpoint MCP a '/mcp' para compatibilidad con VS Code
app.MapMcp("/mcp");

app.Run("http://localhost:3001");

// MCP Tools
[McpServerToolType]
public static class AdafruitIoMcpTools
{
	[McpServerTool, Description("Get all Adafruit IO dashboards.")]
	public static async Task<List<DashboardDto>> GetDashboards(AdafruitIoApiClient client, CancellationToken cancellationToken)
		=> await client.GetDashboardsAsync(cancellationToken);

	[McpServerTool, Description("Get all feeds for the user.")]
	public static async Task<List<FeedDto>> GetFeeds(AdafruitIoApiClient client, CancellationToken cancellationToken)
		=> await client.GetFeedsAsync(cancellationToken);

	[McpServerTool, Description("Get the last value of a feed.")]
	public static async Task<FeedDataDto?> GetFeedLastValue(AdafruitIoApiClient client, [Description("Feed key")] string feedKey, CancellationToken cancellationToken)
		=> await client.GetFeedLastValueAsync(feedKey, cancellationToken);

	[McpServerTool, Description("Set the value of a feed.")]
	public static async Task<bool> SetFeedValue(AdafruitIoApiClient client, [Description("Feed key")] string feedKey, [Description("Value")] string value, CancellationToken cancellationToken)
		=> await client.SetFeedValueAsync(feedKey, value, cancellationToken);

	[McpServerTool, Description("Get all blocks for a dashboard.")]
	public static async Task<List<BlockDto>> GetBlocks(AdafruitIoApiClient client, [Description("Dashboard key")] string dashboardKey, CancellationToken cancellationToken)
		=> await client.GetBlocksAsync(dashboardKey, cancellationToken);
}
