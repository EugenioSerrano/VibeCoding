
using Xunit;
using AdafruitIoApi;
using dotenv.net;

namespace AdafruitIoApi.Tests
{
    public class AdafruitIoClientTests
    {
        private static string? username;
        private static string? apiKey;

        static AdafruitIoClientTests()
        {
            DotEnv.Load();
            username = Environment.GetEnvironmentVariable("USERNAME");
            apiKey = Environment.GetEnvironmentVariable("API_KEY");
        }

        [Fact]
        public async Task GetDashboardsAsync_ReturnsDashboards()
        {
            Assert.False(string.IsNullOrWhiteSpace(username), "USERNAME no configurado en .env");
            Assert.False(string.IsNullOrWhiteSpace(apiKey), "API_KEY no configurado en .env");
            var client = new AdafruitIoClient(username!, apiKey!);
            var dashboards = await client.GetDashboardsAsync();
            Assert.NotNull(dashboards);
            Assert.True(dashboards.Count > 0);
        }

        [Fact]
        public async Task GetFeedsFromDashboardAsync_ReturnsFeeds()
        {
            Assert.False(string.IsNullOrWhiteSpace(username), "USERNAME no configurado en .env");
            Assert.False(string.IsNullOrWhiteSpace(apiKey), "API_KEY no configurado en .env");
            var client = new AdafruitIoClient(username!, apiKey!);
            var dashboards = await client.GetDashboardsAsync();
            var dashboardKey = dashboards[0].Key!;
            var feeds = await client.GetFeedsFromDashboardAsync(dashboardKey);
            Assert.NotNull(feeds);
        }

        [Fact]
        public async Task FeedValue_CanReadAndWrite()
        {
            Assert.False(string.IsNullOrWhiteSpace(username), "USERNAME no configurado en .env");
            Assert.False(string.IsNullOrWhiteSpace(apiKey), "API_KEY no configurado en .env");
            var client = new AdafruitIoClient(username!, apiKey!);
            var dashboards = await client.GetDashboardsAsync();
            var dashboardKey = dashboards[0].Key!;
            var feeds = await client.GetFeedsFromDashboardAsync(dashboardKey);
            var feed = feeds.FirstOrDefault();
            Assert.NotNull(feed);
            var last = await client.GetFeedLastValueAsync(feed!.Key!);
            var newValue = (int.TryParse(last?.Value, out var v) ? (v + 1).ToString() : "1");
            var result = await client.SetFeedValueAsync(feed.Key!, newValue);
            Assert.Equal(newValue, result?.Value);
        }
    }
}
