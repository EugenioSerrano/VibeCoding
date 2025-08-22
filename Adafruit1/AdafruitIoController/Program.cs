using System;
using System.IO;
using AdafruitIoController;

class Program
{
	static void Main(string[] args)
	{
		string[] posiblesRutas = new[] {
			Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".env"),
			Path.Combine(Directory.GetCurrentDirectory(), ".env"),
			Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.FullName ?? "", ".env")
		};
	string envPath = string.Empty;
		foreach (var ruta in posiblesRutas)
		{
			if (File.Exists(ruta))
			{
				envPath = ruta;
				break;
			}
		}
	if (string.IsNullOrEmpty(envPath))
		{
			Console.WriteLine("No se encontró el archivo .env. Rutas buscadas:");
			foreach (var r in posiblesRutas) Console.WriteLine("- " + r);
			return;
		}
		var env = EnvLoader.Load(envPath);
		string username = env.ContainsKey("ADAFRUIT_IO_USERNAME") ? env["ADAFRUIT_IO_USERNAME"] : "";
		string apiKey = env.ContainsKey("ADAFRUIT_IO_KEY") ? env["ADAFRUIT_IO_KEY"] : "";

		if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(apiKey))
		{
			Console.WriteLine("Por favor configura tu ADAFRUIT_IO_USERNAME y ADAFRUIT_IO_KEY en el archivo .env");
			return;
		}

		Console.WriteLine($"Bienvenido, {username}! Tu app está lista para controlar dispositivos en Adafruit IO.\n");

		var client = new AdafruitIoController.AdafruitIoClient(username, apiKey);
		try
		{
			var feeds = client.GetFeedsAsync().GetAwaiter().GetResult();
			if (feeds.Count == 0)
			{
				Console.WriteLine("No se encontraron dispositivos (feeds) en tu cuenta de Adafruit IO.");
				return;
			}
			Console.WriteLine("Dispositivos disponibles:");
			for (int i = 0; i < feeds.Count; i++)
			{
				Console.WriteLine($"[{i + 1}] {feeds[i].Name} (Key: {feeds[i].Key}) - Último valor: {feeds[i].LastValue}");
			}

			Console.Write("\nSelecciona el número del dispositivo a controlar: ");
			if (!int.TryParse(Console.ReadLine(), out int selected) || selected < 1 || selected > feeds.Count)
			{
				Console.WriteLine("Selección inválida.");
				return;
			}
			var feed = feeds[selected - 1];

			Console.WriteLine($"\nSeleccionaste: {feed.Name} (Key: {feed.Key})");
			string value = "";
			if (feed.Key.ToLower() == "valorbarrita")
			{
				Console.WriteLine("Ingresa un valor entre 1 y 100 para la barrita:");
				string input = Console.ReadLine() ?? string.Empty;
				if (!int.TryParse(input, out int num) || num < 1 || num > 100)
				{
					Console.WriteLine("Valor inválido. Debe ser un número entre 1 y 100.");
					return;
				}
				value = num.ToString();
			}
			else
			{
				Console.WriteLine("¿Qué acción deseas realizar?");
				Console.WriteLine("[1] Prender (ON/1)");
				Console.WriteLine("[2] Apagar (OFF/0)");
				Console.Write("Elige 1 o 2: ");
				var action = Console.ReadLine();
				if (action == "1") value = "ON";
				else if (action == "2") value = "OFF";
				else
				{
					Console.WriteLine("Acción inválida.");
					return;
				}
			}

			var ok = client.SendFeedValueAsync(feed.Key, value).GetAwaiter().GetResult();
			if (ok)
			{
				var last = client.GetFeedLastValueAsync(feed.Key).GetAwaiter().GetResult();
				Console.WriteLine($"\nDispositivo actualizado. Estado actual: {last}");
			}
			else
			{
				Console.WriteLine("No se pudo actualizar el dispositivo. Verifica tu conexión y permisos.");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error al obtener los feeds: {ex.Message}");
		}
	}
}
