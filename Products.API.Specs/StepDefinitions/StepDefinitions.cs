using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using NUnit.Framework;
using Products.API.Specs.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.API.Specs.StepDefinitions;

[Binding]
public class StepDefinitions
{
    private readonly ScenarioContext _scenarioContext;
    private readonly HttpClient _webClient;    

    public StepDefinitions(ScenarioContext scenarioContext, WebDriver webDriver)
    {       
        _scenarioContext = scenarioContext;
        _webClient = webDriver.GetWebClient();
    }

    [When(@"accessing the endpoint '([^']*)'")]
    public async Task WhenAccessingTheEndpoint(string endpoint)
    {             
        var response = await _webClient.GetAsync($"{endpoint}");
        _scenarioContext.Add("Response", response);
    }

    [When(@"accessing the endpoint '([^']*)' with query '([^']*)' set to '([^']*)'")]
    public async Task WhenAccessingTheEndpointWithQueryParams(string endpoint, string queryParam, string queryParamValue)
    {

        var response = await _webClient.GetAsync(QueryHelpers.AddQueryString(endpoint, queryParam, queryParamValue));
        _scenarioContext.Add("Response", response);
    }    

    [Then(@"the response should have a valid http status code")]
    public async Task ThenTheResponseShouldBeValid()
    {
        var response = _scenarioContext["Response"].As<HttpResponseMessage>();
        var responseText = await response.Content.ReadAsStringAsync();
        Assert.That(response.IsSuccessStatusCode, Is.True, $"Status Code: {response.StatusCode} {responseText}");
    }

    [Then(@"the result should return '(.*)'")]
    public async Task ThenTheResultShouldReturnMessage(string message)
    {
        var response = _scenarioContext["Response"].As<HttpResponseMessage>();
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.That(responseContent, Is.EqualTo(message));
    }
}
