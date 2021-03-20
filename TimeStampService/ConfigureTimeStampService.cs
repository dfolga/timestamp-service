using NLog;
using System;
using Topshelf;

namespace TimeStampService
{
    class ConfigureTimeStampService
    {
        static void Main(string[] args)
        {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "logs/file.txt" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;


            var rc = HostFactory.Run(x =>
            {
                x.Service<TimeStampService>(s =>
                {
                    s.ConstructUsing(timestampservice => new TimeStampService());
                    s.WhenStarted(timestampservice => timestampservice.Start());
                    s.WhenStopped(timestampservice => timestampservice.Stop());
                });
                x.UseNLog();

                x.RunAsLocalSystem();

                x.SetServiceName("TimeStampService");
                x.SetDisplayName("TimeStampService");
                x.SetDescription("This is a service which is making timestamp files and is logging timestamp using NLog");
            });

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());  
            Environment.ExitCode = exitCode;
        }
    }
}
