using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.Threading.Tasks;

namespace AdafruitIoController
{
    public class McpServerHost
    {
        public static async Task RunAsync(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Logging.AddConsole(consoleLogOptions =>
            {
                consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
            });
            builder.Services
                .AddMcpServer()
                .WithStdioServerTransport()
                .WithToolsFromAssembly();
            await builder.Build().RunAsync();
        }
    }
}
