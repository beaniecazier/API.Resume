using System.Security.Claims;
using Asp.Versioning;
using BeaniesUtilities.APIUtilities.Endpoints;
using Gay.TCazier.DatabaseParser.Endpoints.Extensions;
using Gay.TCazier.Resume.API;
using Gay.TCazier.Resume.API.Auth;
using Gay.TCazier.Resume.API.BackgroundServices;
using Gay.TCazier.Resume.API.CommandLine;
using Gay.TCazier.Resume.API.Extensions;
using Gay.TCazier.Resume.API.Health;
using Gay.TCazier.Resume.API.OutputCache;
using Gay.TCazier.Resume.API.Swagger;
using Gay.TCazier.Resume.API.Versioning;
using Gay.TCazier.Resume.BLL;
using Gay.TCazier.Resume.BLL.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Gay.TCazier.Resume.Contracts.Responses.V1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Core;
using Serilog.Events;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

internal class Program
{
    private static async Task Main(string[] args)
    {
        var defaultApiVersion = new ApiVersion(1, 0);
        List<ApiVersion> versions = new List<ApiVersion>()
        {
            defaultApiVersion,
        };

        var builder = WebApplication.CreateBuilder(args);
        var app = builder.AddApplicationServices(defaultApiVersion, args);

        ////DIRTY HACK, we WILL come back to fix this
        //var scope = app.Services.CreateScope();
        //var context = scope.ServiceProvider.GetRequiredService<ResumeContext>();
        //context.Database.EnsureDeleted();
        //context.Database.EnsureCreated();

        app!.CreateApiVersionSet(versions);

        app.UseSwagger(options =>
        {
            options.RouteTemplate = "resume/swagger/{documentname}/swagger.json";
        });

        app.UseSwaggerUI(c =>
        {
            foreach (var description in app!.DescribeApiVersions())
            {
                c.SwaggerEndpoint($"/resume/swagger/{description.GroupName}/swagger.json", description.GroupName);
            }
            c.RoutePrefix = "resume/swagger";
        });

        app!.UseAuthentication();
        app!.UseAuthorization();

        app!.UseEndpoints<IAPIMarker>();

        if (app!.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app!.Run();

        await Log.CloseAndFlushAsync();
    }
}

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public static class ProgramExtensions
{
    internal static WebApplication? AddApplicationServices(this WebApplicationBuilder? builder,
        ApiVersion defaultApiVersion, string[] args)
    {
        var config = builder!.Configuration;

        var logLevelSwitch = new LoggingLevelSwitch(LogEventLevel.Error);
        if (builder.Environment.IsDevelopment()) logLevelSwitch.MinimumLevel = LogEventLevel.Debug;
        builder.AddLoggingWithSerilog(logLevelSwitch);
        
        var options = CommandLineApplicationExtension.ParseCommandLine(args);
        builder.SetLogLevelFromOptions(options, logLevelSwitch);
        builder.Services.AddSingleton(options);
        
        builder.Services.AddJsonConfigurationOptions();

        //builder.Services.AddSecurity(config);
        builder.Services.AddKeycloakAuthAPI(config);

        builder.Services.AddHostedService<StartupBackgroundService>();

        builder.Services.AddApiVersioningSettings(defaultApiVersion);

        builder.Services.AddOutputAndResponseCacheing();

        builder.Services.AddHealthCheckServices();

        builder.Services.ConfigureAndAddSwagger(config);

        builder.Services.AddApplication();

        builder.Services.AddDatabase(config);

        builder.Services.AddEndpoints<IAPIMarker>(config);

        return builder.Build();
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member