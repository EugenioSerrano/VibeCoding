
using AdafruitIoClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

// Cargar configuración desde la carpeta del ejecutable
var configPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
builder.Configuration.AddJsonFile(configPath, optional: false, reloadOnChange: true);
builder.Services.Configure<AdafruitIoOptions>(builder.Configuration.GetSection("AdafruitIo"));

// HttpClient y cliente API
builder.Services.AddHttpClient<AdafruitIoApiClient>();

var host = builder.Build();
var logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Main");
var api = host.Services.GetRequiredService<AdafruitIoApiClient>();

while (true)
{
	Console.WriteLine("\n--- Adafruit IO Console ---");
	Console.WriteLine("1. Listar dashboards");
	Console.WriteLine("2. Listar feeds");
	Console.WriteLine("3. Consultar estado de un feed");
	Console.WriteLine("4. Modificar valor de un feed");
	Console.WriteLine("0. Salir");
	Console.Write("Opción: ");
	var opt = Console.ReadLine();
	try
	{
		switch (opt)
		{
			case "1":
				var dashboards = await api.GetDashboardsAsync();
				foreach (var d in dashboards)
					Console.WriteLine($"- {d.Name} (Key: {d.Key})");
				break;
			case "2":
				var feeds = await api.GetFeedsAsync();
				foreach (var f in feeds)
					Console.WriteLine($"- {f.Name} (Key: {f.Key}) Último valor: {f.Last_Value}");
				break;
			case "3":
				Console.Write("Feed Key: ");
				var feedKey = Console.ReadLine();
				var data = await api.GetFeedLastValueAsync(feedKey!);
				if (data != null)
					Console.WriteLine($"Último valor: {data.Value} (Fecha: {data.Created_At})");
				else
					Console.WriteLine("No se encontró el feed o no tiene datos.");
				break;
			case "4":
				Console.Write("Feed Key: ");
				var key = Console.ReadLine();
				Console.Write("Nuevo valor: ");
				var value = Console.ReadLine();
				var ok = await api.SetFeedValueAsync(key!, value!);
				Console.WriteLine(ok ? "Valor actualizado correctamente." : "Error al actualizar valor.");
				break;
			case "0":
				return;
			default:
				Console.WriteLine("Opción no válida.");
				break;
		}
	}
	catch (Exception ex)
	{
		logger.LogError(ex, "Error en la operación");
		Console.WriteLine($"Error: {ex.Message}");
	}
}
