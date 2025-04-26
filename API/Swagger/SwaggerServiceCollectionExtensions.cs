using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Gay.TCazier.Resume.API.Swagger;

#pragma warning disable CS1591

public static class SwaggerServiceCollectionExtensions
{
    public static IServiceCollection ConfigureAndAddSwagger(this IServiceCollection services, ConfigurationManager config)
    {
        var host = config["Keycloak:auth-server-url"];
        var realm = config["Keycloak:realm"];
        var wellKnownURL = new Uri($"{host}/realms/{realm}/.well-known/openid-configuration");
        
        // var keycloakScopes = new Dictionary<string, string>
        // {
        //     { "openid", "openid" },
        //     { "profile", "profile" }
        // };
        // var keycloakFlow = new OpenApiOAuthFlow
        // { 
        //     AuthorizationUrl = new Uri(authURL),
        //     Scopes = keycloakScopes
        // };
        // var oauthFlows = new OpenApiOAuthFlows { Implicit = keycloakFlow };
        var apiSecurityScheme = new OpenApiSecurityScheme
        {
            Name = "Keycloak",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.OpenIdConnect,
            OpenIdConnectUrl = wellKnownURL, 
            Scheme = "bearer",
            BearerFormat = "JWT",
            Reference = new OpenApiReference()
            {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme,
            }
        };
        
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        
        var securityRequirement = MakeNewOpenApiSecurityRequirement(apiSecurityScheme);
        
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        
        services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();
            options.OperationFilter<SwaggerDefaultValues>();
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            options.AddSecurityDefinition(apiSecurityScheme.Reference.Id, apiSecurityScheme);
            options.AddSecurityRequirement(securityRequirement);
        });

        return services;
    }

    private static OpenApiSecurityRequirement MakeNewOpenApiSecurityRequirement(OpenApiSecurityScheme apiSecurityScheme)
    {
        // var reference = new OpenApiReference
        // {
        //     Id = "Keycloak",
        //     Type = ReferenceType.SecurityScheme,
        // };
        //
        // var securityScheme = new OpenApiSecurityScheme
        // {
        //     Reference = reference,
        //     In = ParameterLocation.Header,
        //     Name = "Bearer",
        //     Scheme = "Bearer",
        // };

        return new OpenApiSecurityRequirement
        {
            {
                apiSecurityScheme,
                []
            }
        };
    }
}

#pragma warning restore CS1591