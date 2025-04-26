using CommandLine;

namespace Gay.TCazier.Resume.BLL.CommandLine;

#pragma warning disable CS1591

public class CMDOptions
{
    // Omitting long name, defaults to name of property, ie "--verbose"
    [Option(
        'v',
        "verbose",
        Default = 2,
        HelpText = 
            """
            Prints all messages to standard output. The default behaviour without this setting is to only log out errors

            When passed a value the verbosity is set to that level.  These levels are based on the Serilog Logging
            levels. Provided is this table taken from, https://github.com/serilog/serilog/wiki/Configuration-Basics
                0.Verbose: Verbose is the noisiest level, rarely (if ever) enabled for a production app.
                1.Debug: Debug is used for internal system events that are not necessarily observable from the outside,
                    but useful when determining how something happened.
                2.Information: Information events describe things happening in the system that correspond to its
                    responsibilities and functions. Generally these are the observable actions the system can perform.
                3.Warning: When service is degraded, endangered, or may be behaving outside of its expected parameters,
                    Warning level events are used.
                4.Error: When functionality is unavailable or expectations broken, an Error event is used.
                5.Fatal: The most critical level, Fatal events demand immediate attention.
            """)]
    public int Verbose { get; set; }
    public bool UseVerboseLogging { get; set; } = false;


    

    //[Option("dbconn", Required = true)]
    //public string DBConnStr { get; set; }

    //[Option('u', Required = true)]
    //public string BaseUrl {  get; set; }

    [Option("versions", Required = true, Separator = ';')]
    public IEnumerable<string> VersionsToLoad { get; set; } = new List<string>() { "v1" };

    [Option('h', "health-host", Required = true)]
    public string HealthChecksHostAddress { get; set; } = "*";
    
    



    [Option("ready", Default = 5051)]
    public int ReadyCheckPort { get; set; }

    [Option("liveness", Default = 5052)]
    public int LivenessCheckPort { get; set; }

    [Option("startup", Default = 5050)]
    public int StartupCheckPort { get; set; }

    [Option(Default = 5000)]
    public int HttpPort { get; set; }

    [Option(Default = 5001)]
    public int HttpsPort { get; set; }

    [Option('c', "cache-expire", Default = 60)]
    public int CacheExpirationTimeInMinutes { get; set; }

    //[Option("startuptime", Default = 10)]
    //public int FakedStartupDurationInSeconds { get; set; }

    [Option(Default = "Tiabeanie Cazier")]
    public string ContactName { get; set; } = "Tiabeanie Cazier";

    [Option(Default = "beanieroxiicazier@gmail.com")]
    public string ContactEmail { get; set; } = "beanieroxiicazier@gmail.com";

    [Option(Default = "https://tcazier.gay/contact")]
    public string ContactURL { get; set; } = "https://tcazier.gay/contact";

    [Option(Default = "https://tcazier.gay/terms")]
    public string TermsOfServiceURL { get; set; } = "https://tcazier.gay/terms";

    //loging information like min level

}

#pragma warning restore CS1591