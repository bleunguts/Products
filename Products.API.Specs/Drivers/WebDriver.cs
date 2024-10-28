using Microsoft.AspNetCore.Mvc.Testing;

namespace Products.API.Specs.Drivers;

public class WebDriver
{
    private readonly WebApplicationFactory<Startup> _factory;
    private readonly string _apiServerUri;
    private readonly string _jwtKey;

    public WebDriver(WebApplicationFactory<Startup> factory, string apiServerUri, string jwtKey)
    {
        _factory = factory;
        _apiServerUri = apiServerUri;
        _jwtKey = jwtKey;
    }

    public HttpClient GetWebClient()
    {
        var webClient = _factory.CreateDefaultClient(new Uri(_apiServerUri));
        webClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_jwtKey}");

        return webClient;
    }
}

