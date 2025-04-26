using Gay.TCazier.Resume.API.CommandLine;
using Gay.TCazier.Resume.BLL.CommandLine;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Gay.TCazier.Resume.API.Extensions;

public static class SerilogWebApplicationBuilderExtension
{
    public static void SetLogLevelFromOptions(this WebApplicationBuilder builder, CMDOptions options,
        LoggingLevelSwitch levelSwitch)
    {
        if (builder.Environment.IsDevelopment()) return;
        if (!CommandLineApplicationExtension.BeVerbose) return;

        switch (options.Verbose)
        {
            case 0: levelSwitch.MinimumLevel = LogEventLevel.Verbose; break;
            case 1: levelSwitch.MinimumLevel = LogEventLevel.Debug; break;
            case 2: levelSwitch.MinimumLevel = LogEventLevel.Information; break;
            case 3: levelSwitch.MinimumLevel = LogEventLevel.Warning; break;
            case 4: levelSwitch.MinimumLevel = LogEventLevel.Error; break;
            case 5: levelSwitch.MinimumLevel = LogEventLevel.Fatal; break;
            default: 
                throw new ArgumentOutOfRangeException("options.Verbose",
                    options.Verbose,
                    "Invalid Serilog logging level found while trying to set the log level");
        }
    }
    
    public static void AddLoggingWithSerilog(this WebApplicationBuilder builder, LoggingLevelSwitch levelSwitch)
    {
        Serilog.ILogger logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .MinimumLevel.ControlledBy(levelSwitch)
            .WriteTo.File("log.txt", rollingInterval:RollingInterval.Day, rollOnFileSizeLimit:true)
            .CreateLogger();
        Log.Logger = logger;
        builder.Host.UseSerilog();
    }
}