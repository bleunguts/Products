﻿using BoDi;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Products.API.Services;
using Products.API.Specs.Drivers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Products.API.Specs;

[Binding]
public class StartupHook
{
    private static IConfiguration Config;
    private readonly IObjectContainer _container;

    public StartupHook(IObjectContainer iocContainer)
    {
        _container = iocContainer;
    }

    [BeforeScenario]
    public async Task Setup()
    {
        ConfigurationFrom("appSettings.json");
        _container.RegisterInstanceAs<IConfiguration>(Config);

        var factory = BuildWebApplicationFactory();
        _container.RegisterInstanceAs(factory);

        var apiServerUri = Config["ApiServerUri"];
        string jwtKey = await GetJwtKeyFromApiServer(factory, apiServerUri);
        var webDriver = new WebDriver(factory, apiServerUri, jwtKey);

        _container.RegisterInstanceAs(webDriver);

        var jsonFilename = Config["backingDataStore:jsonFileName"];        
        var productJsonDataStore = new ProductJsonDataStoreService(Options.Create<ApplicationSettings>(new ApplicationSettings { DataStoreJsonFileName = jsonFilename }));
        productJsonDataStore.Drop();
        _container.RegisterInstanceAs<IProductJsonDataStoreService>(productJsonDataStore);
    }    

    private static void ConfigurationFrom(string configFileName)
    {
        if (Config == null)
        {
            Config = new ConfigurationBuilder()
                .AddJsonFile(configFileName, optional: false, reloadOnChange: true)
                .Build();
        }
    }

    private static async Task<string> GetJwtKeyFromApiServer(WebApplicationFactory<Startup> factory, string apiServerUri)
    {
        var webClient = factory.CreateDefaultClient(new Uri(apiServerUri));
        return await webClient.GetStringAsync($"api/auth");
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

