
using AdafruitIoApi;

Console.WriteLine("Adafruit IO Console App");

// Reemplaza estos valores por los tuyos
string username = "TU_USUARIO";
string apiKey = "TU_API_KEY";

var client = new AdafruitIoClient(username, apiKey);

// 1. Listar dashboards
echo("Obteniendo dashboards...");
var dashboards = await client.GetDashboardsAsync();
foreach (var dash in dashboards)
{
	Console.WriteLine($"Dashboard: {dash.name} (key: {dash.key})");
}

if (dashboards.Count == 0)
{
	Console.WriteLine("No hay dashboards disponibles.");
	return;
}

// 2. Seleccionar el primer dashboard y listar sus feeds
var dashboardKey = dashboards[0].key!;
echo($"\nObteniendo feeds del dashboard '{dashboards[0].name}'...");
var feeds = await client.GetFeedsFromDashboardAsync(dashboardKey);
foreach (var feed in feeds)
{
	Console.WriteLine($"Feed: {feed.name} (key: {feed.key}) | Último valor: {feed.last_value}");
}

if (feeds.Count == 0)
{
	Console.WriteLine("No hay feeds en este dashboard.");
	return;
}

// 3. Consultar el estado actual del primer feed
echo($"\nConsultando estado actual del feed '{feeds[0].name}'...");
var currentValue = await client.GetFeedCurrentValueAsync(feeds[0].key!);
Console.WriteLine($"Valor actual: {currentValue}");

// 4. Modificar el valor del primer feed
echo($"\nIntroduce un nuevo valor para el feed '{feeds[0].name}': ");
var newValue = Console.ReadLine();
if (!string.IsNullOrWhiteSpace(newValue))
{
	var ok = await client.SetFeedValueAsync(feeds[0].key!, newValue);
	if (ok)
		Console.WriteLine("Valor actualizado correctamente.");
	else
		Console.WriteLine("Error al actualizar el valor.");
}
else
{
	Console.WriteLine("No se modificó el valor.");
}

void echo(string msg) => Console.WriteLine("\n=== " + msg + " ===\n");
