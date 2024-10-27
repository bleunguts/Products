using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NUnit.Framework;
using System;
using TechTalk.SpecFlow;

namespace Products.API.Specs.StepDefinitions
{
    [Binding]
    public class ProductsStepDefinitions
    {
        private const string BaseAddress = "http://localhost/";
        private readonly WebApplicationFactory<Startup> _webApplicationFactory;
        private HttpClient _webClient;
        private HttpResponseMessage _response;

        public ProductsStepDefinitions(WebApplicationFactory<Startup> webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
        }

        [Given(@"I am a web client")]
        public void IamAWebClient()
        {
            _webClient = _webApplicationFactory.CreateDefaultClient(new Uri(BaseAddress));
        }

        [When(@"accessing health endpoint (.*)")]
        public async Task WhenAccessingHealthEndpointHealthCheckAsync(string endpoint)
        {
            _response = await _webClient.GetAsync($"{endpoint}");
        }

        [Then(@"the response should be valid")]
        public async Task ThenTheResponseShouldBeValidAsync()
        {
            Assert.That(_response.IsSuccessStatusCode, Is.True);

            var responseContent = await _response.Content.ReadAsStringAsync();
            Assert.That(responseContent, Is.EqualTo("Healthy"));
        }

        [When(@"accessing the default endpoint (.*)")]
        public async Task WhenAccessingTheDefaultEndpointAsync(string endpoint)
        {
            _response = await _webClient.GetAsync($"{endpoint}");
        }

        [Then(@"the result should return (.*)")]
        public async Task ThenTheResultShouldReturnMessageAsync(string message)
        {
            Assert.That(_response.IsSuccessStatusCode, Is.True);

            var responseContent = await _response.Content.ReadAsStringAsync();
            Assert.That(responseContent, Is.EqualTo(message));
        }
    }
}
