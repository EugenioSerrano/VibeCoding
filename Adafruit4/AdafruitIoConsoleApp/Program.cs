
using AdafruitIoApi;
using dotenv.net;

namespace AdafruitIoConsoleApp
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			DotEnv.Load();
			var username = Environment.GetEnvironmentVariable("USERNAME") ?? string.Empty;
			var apiKey = Environment.GetEnvironmentVariable("API_KEY") ?? string.Empty;
			if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(apiKey))
			{
				Console.WriteLine("Faltan USERNAME o API_KEY en el archivo .env");
				return;
			}
			var client = new AdafruitIoClient(username, apiKey);

			Console.WriteLine("Obteniendo dashboards...");
			var dashboards = await client.GetDashboardsAsync();
			foreach (var dash in dashboards)
				Console.WriteLine($"Dashboard: {dash.Name} (Key: {dash.Key})");

			Console.Write("Ingresa el dashboardKey a consultar: ");
			var dashboardKey = Console.ReadLine() ?? string.Empty;
			var feeds = await client.GetFeedsFromDashboardAsync(dashboardKey);
			Console.WriteLine($"Feeds en el dashboard {dashboardKey}:");
			foreach (var feed in feeds)
				Console.WriteLine($"Feed: {feed?.Name} (Key: {feed?.Key})");

			Console.Write("Ingresa el feedKey a consultar estado: ");
			var feedKey = Console.ReadLine() ?? string.Empty;
			var last = await client.GetFeedLastValueAsync(feedKey);
			Console.WriteLine($"Último valor: {last?.Value} (Fecha: {last?.CreatedAt})");

			Console.Write("¿Quieres modificar el valor? (s/n): ");
			if (Console.ReadLine()?.Trim().ToLower() == "s")
			{
				Console.Write("Nuevo valor: ");
				var newValue = Console.ReadLine() ?? string.Empty;
				var result = await client.SetFeedValueAsync(feedKey, newValue);
				Console.WriteLine($"Valor actualizado: {result?.Value} (Fecha: {result?.CreatedAt})");
			}
		}
	}
}
