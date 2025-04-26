using CommandLine;
using Gay.TCazier.Resume.API.OutputCache;
using Gay.TCazier.Resume.API.Swagger;
using Gay.TCazier.Resume.BLL.CommandLine;
using Serilog;

namespace Gay.TCazier.Resume.API.CommandLine;

#pragma warning disable CS1591

public static class CommandLineApplicationExtension
{
    public static bool BeVerbose = false;
    
    public static CMDOptions ParseCommandLine(string[] args)
    {
        if (args.Contains("-v") || args.Contains("--verbose")) BeVerbose = true;
        var argsList = args.Where(x => !x.Contains("--applicationName") &&
                                    !x.Contains("--environment") &&
                                    !x.Contains("--contentRoot"));
        
        if (args.Contains("-v") || args.Contains("--verbose")) BeVerbose = true;
        
        var results = Parser.Default.ParseArguments<CMDOptions>(argsList)
            .WithParsed<CMDOptions>(RunOptions)
            .WithNotParsed(HandleParseError);

        return results.Value;
    }

    static void RunOptions(CMDOptions opts)
    {
        OutputCacheServiceExtensions.OutputCacheExpirationInMinutes = opts.CacheExpirationTimeInMinutes;
        //StartupBackgroundService.FakedStartupDurationInSeconds = results.Value.FakedStartupDurationInSeconds;

        ConfigureSwaggerOptions.ContactName = opts.ContactName;
        ConfigureSwaggerOptions.ContactURL = opts.ContactURL;
        ConfigureSwaggerOptions.ContactEmail = opts.ContactEmail;
        ConfigureSwaggerOptions.TermsOfServiceURL = opts.TermsOfServiceURL;
    }

    static void HandleParseError(IEnumerable<Error> errs)
    {
        foreach (var error in errs) Log.Error("", error);
        throw new Exception("One or more errors occurred.\n" +
                            "The provided cli arguments are not valid.\n" +
                            "Terminating program.");
    }
}

#pragma warning restore CS1591