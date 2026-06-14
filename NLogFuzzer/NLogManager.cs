using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using System.Text;

namespace NLogFuzzer
{
    internal class NLogManager
    {
        public static void SetupNLog(int port = 6666, bool logToConsole = false)
        {
            var config = new LoggingConfiguration();

            if (logToConsole)
            {
                var consoleTarget = new ConsoleTarget
                {
                    Name = "console",
                    Layout = "${longdate}|${level:uppercase=true}|${logger}|${message}",
                };
                config.AddRule(LogLevel.Debug, LogLevel.Fatal, consoleTarget, "*");
            }

            var tcpNetworkTarget = new NetworkTarget
            {
                Address = $"tcp4://127.0.0.1:{port}",
                Encoding = Encoding.UTF8,
                Name = "NLogViewer",
                Layout = new Log4JXmlEventLayout()
            };

            var tcpNetworkRule = new LoggingRule("*", LogLevel.Trace, tcpNetworkTarget);
            config.LoggingRules.Add(tcpNetworkRule);


            LogManager.Configuration = config;
        }

    }
}
