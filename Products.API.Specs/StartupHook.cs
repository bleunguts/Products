using BoDi;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Products.API.Specs;

[Binding]
public class StartupHook
{
    private readonly IObjectContainer _container;

    public StartupHook(IObjectContainer iocContainer)
    {
        _container = iocContainer;
    }

    [BeforeScenario]
    public void Setup()
    {
        var factory = BuildWebApplicationFactory();
        _container.RegisterInstanceAs(factory);
    }

    WebApplicationFactory<Startup> BuildWebApplicationFactory() => 
        new WebApplicationFactory<Startup>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    // here is where set config such as database for testing
                });
            });
}
