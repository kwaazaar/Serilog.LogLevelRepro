using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.LogLevelRepro
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set up logging
            ILoggerFactory loggerFactory = new LoggerFactory();

            // MS console
            loggerFactory.AddConsole(LogLevel.Trace); // Trace=Most detailed on Microsoft.Extensions.Logging

            // Serilog console
            var configuration = new LoggerConfiguration()
                .Enrich.WithProperty("servername", Environment.MachineName)
                .WriteTo.ColoredConsole(restrictedToMinimumLevel: Events.LogEventLevel.Verbose, // Verbose=Most detailed on Serilog
                    outputTemplate: "SERILOG: [{Level}]: {Message}{NewLine}");  // Specific template, to make it stand out on the console
            loggerFactory.AddSerilog(configuration.CreateLogger());

            // Log on all levels
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogCritical("**critical**");
            logger.LogDebug("**debug**"); // Not logged by Serilog!
            logger.LogError("**error**");
            logger.LogInformation("**info**");
            logger.LogTrace("**trace**"); // Not logged by Serilog!
            logger.LogWarning("**warning**");

            Console.ReadKey();
        }
    }
}
