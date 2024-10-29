using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Products.API.Entities;
using Products.API.Services;
using Products.API.Specs.Drivers;
using Products.API.Specs.Helpers;
using Products.Model;
using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using TechTalk.SpecFlow;

namespace Products.API.Specs.StepDefinitions;

[Binding]
public class ProductsStepDefinitions
{
    private readonly ScenarioContext _scenarioContext;
    private readonly IProductJsonDataStoreService _jsonDataStore;
    private readonly HttpClient _webClient;

    private readonly List<Product> someProducts = ProductHelper.GenerateProducts(3).ToList();

    public ProductsStepDefinitions(ScenarioContext scenarioContext, WebDriver webDriver, IProductJsonDataStoreService jsonDataStore)            
    {
        _scenarioContext = scenarioContext;
        _jsonDataStore = jsonDataStore;
        _webClient = webDriver.GetWebClient();
    }

    [When(@"Creating a new Product with name '([^']*)'")]
    public async Task WhenCreatingANewProductWithName(string aProductName)
    {
        var productDto = new ProductDto { Name = aProductName };
        HttpResponseMessage response = await PostCreateProductRequest(productDto);

        _scenarioContext["Response"] = response;
    }      

    [When(@"Creating a new Product with name '([^']*)' with colour '([^']*)'")]
    public async Task WhenCreatingANewProductWithName(string aProductName, string colour)
    {
        var productDto = new ProductDto { Name = aProductName, Colour = Color.FromName(colour)};
        HttpResponseMessage response = await PostCreateProductRequest(productDto);

        _scenarioContext["Response"] = response;
    }

    [When(@"Creating a new Product without required arguments")]
    public async Task WhenCreatingANewProduct()
    {
        var productDto = new ProductDto();
        HttpResponseMessage response = await PostCreateProductRequest(productDto);

        _scenarioContext["Response"] = response;
    }

    [Given(@"there are some products in the repository")]
    public void GivenThereAreSomeProductsInTheRepository()
    {            
        _jsonDataStore.CreateProducts(someProducts);
    }

    [Then(@"the result should return some products")]
    public async Task ThenTheResultShouldReturnSomeProducts()
    {
        List<ProductDto> productDtos = await GetProductsFromCachedResponse();

        Assert.That(productDtos.Count, Is.EqualTo(3));
        Assert.That(productDtos.Select(x => x.Name), Is.EquivalentTo(someProducts.Select(x => x.Name)));
    }

    [Given(@"there is a '([^']*)' product in the repository")]
    public void GivenThereIsAColouredProductInTheRepository(string colour)
    {            
        var aColouredProduct = new Product { Name = "Coloured Product", Colour = colour };
        var productsWithAColouredProduct = ProductHelper.GenerateProducts(25, "anotherColour").Concat(new[] { aColouredProduct });

        _jsonDataStore.CreateProducts(productsWithAColouredProduct);            
    }

    [Then(@"the result should return a product which has colour '([^']*)'")]
    public async void ThenTheResultShouldReturnAProductWhichHasColour(string colour)
    {
        var products = await GetProductsFromCachedResponse();

        Assert.That(products.Count(p => p.Colour.Equals(Color.FromName(colour))), Is.EqualTo(1));
    }

    [Then(@"the result should return no products")]
    public async Task ThenTheResultShouldReturnNoProductsAsync()
    {
        List<ProductDto> productDtos = await GetProductsFromCachedResponse();

        Assert.That(productDtos, Is.Empty); 
    }

    [Then(@"the response should have a '([^']*)' http status code")]
    public void ThenTheResponseShouldHaveAHttpStatusCode(string statusCode)
    {
        var httpStatusCode = Enum.Parse<HttpStatusCode>(statusCode);

        var response = _scenarioContext["Response"].As<HttpResponseMessage>();
        Assert.That(response.StatusCode, Is.EqualTo(httpStatusCode));
    }

    [Then(@"the response should contain error message detailing problem '([^']*)'")]
    public async Task ThenTheResponseShouldContainErrorMessageDetailingProblemAsync(string error)
    {
        var response = _scenarioContext["Response"].As<HttpResponseMessage>();

        var responseText = await response.Content.ReadAsStringAsync();
        Assert.That(responseText.Contains(error));
    }

    [Then(@"the response should return a 'NotFound' http status code and an error message that contains '([^']*)'")]
    public async void ThenTheResultShouldReturnANotFound(string error)
    {
        var response = _scenarioContext["Response"].As<HttpResponseMessage>();
        var responseText = await response.Content.ReadAsStringAsync();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        Assert.That(responseText.Contains(error), Is.True, responseText);

    }

    private async Task<HttpResponseMessage> PostCreateProductRequest(ProductDto productDto)
    {
        var jsonData = JsonSerializer.Serialize(productDto);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        var response = await _webClient.PostAsync("api/Products/CreateProduct", content);
        return response;
    }
    private async Task<List<ProductDto>> GetProductsFromCachedResponse()
    {
        var response = _scenarioContext["Response"].As<HttpResponseMessage>();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<ProductDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});        
    }
}
