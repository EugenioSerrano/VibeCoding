
using AdafruitIoApiClient;

Console.WriteLine("Adafruit IO Console App");

// Configuración: pide username y API Key al usuario
Console.Write("Username de Adafruit IO: ");
var username = Console.ReadLine() ?? string.Empty;
Console.Write("API Key de Adafruit IO: ");
var apiKey = Console.ReadLine() ?? string.Empty;

var client = new AdafruitIoClient(username, apiKey);

while (true)
{
	Console.WriteLine("\nOpciones:");
	Console.WriteLine("1. Listar dashboards");
	Console.WriteLine("2. Listar feeds de un dashboard");
	Console.WriteLine("3. Consultar estado de un feed");
	Console.WriteLine("4. Modificar valor de un feed");
	Console.WriteLine("0. Salir");
	Console.Write("Selecciona una opción: ");
	var opt = Console.ReadLine();
	if (opt == "0") break;

	switch (opt)
	{
		case "1":
			var dashboards = await client.GetDashboardsAsync();
			Console.WriteLine("\nDashboards:");
			foreach (var d in dashboards)
				Console.WriteLine($"- {d.Name} (Key: {d.Key})");
			break;
		case "2":
			Console.Write("Dashboard Key: ");
			var dashKey = Console.ReadLine() ?? string.Empty;
			var blocks = await client.GetBlocksAsync(dashKey);
			Console.WriteLine($"\nFeeds asociados a los bloques del dashboard '{dashKey}':");
			var feedKeys = new HashSet<string>();
			foreach (var block in blocks)
			{
				foreach (var bf in block.BlockFeeds)
				{
					if (bf.Feed != null && feedKeys.Add(bf.Feed.Key))
						Console.WriteLine($"- {bf.Feed.Name} (Key: {bf.Feed.Key})");
				}
			}
			if (feedKeys.Count == 0) Console.WriteLine("No hay feeds asociados.");
			break;
		case "3":
			Console.Write("Feed Key: ");
			var feedKey = Console.ReadLine() ?? string.Empty;
			var data = await client.GetFeedLastDataAsync(feedKey);
			if (data != null)
				Console.WriteLine($"Último valor: {data.Value} (Fecha: {data.CreatedAt})");
			else
				Console.WriteLine("Feed no encontrado o sin datos. Verifica el nombre del feed.");
			break;
		case "4":
			Console.Write("Feed Key: ");
			var feedKeySet = Console.ReadLine() ?? string.Empty;
			Console.Write("Nuevo valor: ");
			var newValue = Console.ReadLine() ?? string.Empty;
			var ok = await client.SendFeedDataAsync(feedKeySet, newValue);
			Console.WriteLine(ok ? "Valor enviado correctamente." : "Error al enviar el valor.");
			break;
		default:
			Console.WriteLine("Opción no válida.");
			break;
	}
}
Console.WriteLine("Fin.");
