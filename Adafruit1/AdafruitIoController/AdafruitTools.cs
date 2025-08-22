using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Threading.Tasks;

namespace AdafruitIoController
{
    [McpServerToolType]
    public static class AdafruitTools
    {
        [McpServerTool, Description("Prende un dispositivo (feed) de Adafruit IO")]
        public static async Task<string> Prender(string feedKey)
        {
            var env = EnvLoader.Load(".env");
            var client = new AdafruitIoClient(env["ADAFRUIT_IO_USERNAME"], env["ADAFRUIT_IO_KEY"]);
            var ok = await client.SendFeedValueAsync(feedKey, "ON");
            return ok ? $"Dispositivo '{feedKey}' prendido" : $"No se pudo prender '{feedKey}'";
        }

        [McpServerTool, Description("Apaga un dispositivo (feed) de Adafruit IO")]
        public static async Task<string> Apagar(string feedKey)
        {
            var env = EnvLoader.Load(".env");
            var client = new AdafruitIoClient(env["ADAFRUIT_IO_USERNAME"], env["ADAFRUIT_IO_KEY"]);
            var ok = await client.SendFeedValueAsync(feedKey, "OFF");
            return ok ? $"Dispositivo '{feedKey}' apagado" : $"No se pudo apagar '{feedKey}'";
        }

        [McpServerTool, Description("Ajusta el valor de una barrita (feed tipo slider) de Adafruit IO")]
        public static async Task<string> AjustarBarrita(string feedKey, int valor)
        {
            if (valor < 1 || valor > 100)
                return "El valor debe estar entre 1 y 100";
            var env = EnvLoader.Load(".env");
            var client = new AdafruitIoClient(env["ADAFRUIT_IO_USERNAME"], env["ADAFRUIT_IO_KEY"]);
            var ok = await client.SendFeedValueAsync(feedKey, valor.ToString());
            return ok ? $"Barrita '{feedKey}' ajustada a {valor}" : $"No se pudo ajustar '{feedKey}'";
        }
    }
}
