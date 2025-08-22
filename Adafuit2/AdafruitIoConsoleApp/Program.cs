
using System;
using System.IO;
using System.Threading.Tasks;
using AdafruitIoConsoleApp.Services;
using AdafruitIoConsoleApp.Models;
using System.Collections.Generic;

namespace AdafruitIoConsoleApp
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var config = LoadConfig();
			if (config == null)
			{
				Console.WriteLine("Configura tu .env con ADAFRUIT_IO_USERNAME y ADAFRUIT_IO_KEY");
				return;
			}

			var service = new AdafruitIoService(config.Value.Username, config.Value.ApiKey);

			while (true)
			{
				Console.WriteLine("\n1. Listar feeds\n2. Estado de un feed\n3. Enviar comando a feed\n4. Salir");
				Console.Write("Selecciona una opción: ");
				var opt = Console.ReadLine();
				if (opt == "1")
				{
					var feeds = await service.GetFeedsAsync();
					foreach (var feed in feeds)
					{
						Console.WriteLine($"- {feed.Name} (key: {feed.Key}) Último valor: {feed.LastValue}");
					}
				}
				else if (opt == "2")
				{
					Console.Write("Feed key: ");
					var key = Console.ReadLine();
					var data = await service.GetFeedDataAsync(key ?? "", 1);
					if (data.Count > 0)
						Console.WriteLine($"Último valor: {data[0].Value} (fecha: {data[0].CreatedAt})");
					else
						Console.WriteLine("No hay datos para este feed.");
				}
				else if (opt == "3")
				{
					Console.Write("Feed key: ");
					var key = Console.ReadLine();
					Console.Write("Nuevo valor: ");
					var value = Console.ReadLine();
					var ok = await service.SendFeedDataAsync(key ?? "", value ?? "");
					Console.WriteLine(ok ? "Comando enviado correctamente." : "Error al enviar comando.");
				}
				else if (opt == "4")
				{
					break;
				}
				else
				{
					Console.WriteLine("Opción no válida.");
				}
			}
		}

		static (string Username, string ApiKey)? LoadConfig()
		{
			var envPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".env");
			if (!File.Exists(envPath))
				return null;
			var lines = File.ReadAllLines(envPath);
			string? username = null, apiKey = null;
			foreach (var line in lines)
			{
				if (line.StartsWith("ADAFRUIT_IO_USERNAME="))
					username = line.Substring("ADAFRUIT_IO_USERNAME=".Length).Trim();
				if (line.StartsWith("ADAFRUIT_IO_KEY="))
					apiKey = line.Substring("ADAFRUIT_IO_KEY=".Length).Trim();
			}
			if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(apiKey))
				return null;
			return (username, apiKey);
		}
	}
}
