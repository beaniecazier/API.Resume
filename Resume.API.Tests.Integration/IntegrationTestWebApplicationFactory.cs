using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Testing;
using Gay.TCazier.Resume.API;

namespace Resume.API.Tests.Integration;

public class IntegrationTestWebApplicationFactory :
    WebApplicationFactory<IAPIMarker>
{

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("versions", "v1.0");
        builder.UseSetting("health-host", "*");
        builder.UseSetting("verbose", "true");
        builder.UseSetting("startup", "5050");
        builder.UseSetting("ready", "5051");
        builder.UseSetting("liveness", "5052");
        builder.UseSetting("cache-expire", "60");
        base.ConfigureWebHost(builder);
    }
}
