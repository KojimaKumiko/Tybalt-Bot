using Discord;
using Discord.WebSocket;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TybaltBot.Services
{
    public class LoggingService
    {
        public ILogger Logger { get; set; }

        public LoggingService(DiscordSocketClient client)
        {
            _ = client ?? throw new ArgumentNullException(nameof(client));

            client.Log += LogAsync;

            Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .CreateLogger();
        }

        private Task LogAsync(LogMessage message)
        {
            Logger.Write(GetLogLevel(message.Severity), message.Message);

            return Task.CompletedTask;
        }

        private static LogEventLevel GetLogLevel(LogSeverity severity) => (LogEventLevel)Math.Abs((int)severity - 5);
    }
}
