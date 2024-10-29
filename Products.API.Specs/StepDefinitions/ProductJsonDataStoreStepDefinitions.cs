using NUnit.Framework;
using Products.API.Entities;
using Products.API.Services;
using Products.API.Specs.Helpers;
using Products.API.Utils;
using System;
using System.Drawing;
using TechTalk.SpecFlow;

namespace Products.API.Specs.StepDefinitions
{
    [Binding]    
    public class ProductJsonDataStoreStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IProductJsonDataStoreService _productJsonDataStore;
        private readonly JsonDataStorePeekService _jsonFileService;

        public ProductJsonDataStoreStepDefinitions(
            ScenarioContext scenarioContext, 
            IProductJsonDataStoreService productJsonDataStore,
            JsonDataStorePeekService jsonFileService)
        {
            _scenarioContext = scenarioContext;
            _productJsonDataStore = productJsonDataStore;
            _jsonFileService = jsonFileService;
        }

        [Given(@"there are no products in the repository")]
        public async Task GivenEmptyDataStore()
        {
            await _productJsonDataStore.DropDataStore_UsedForTests();
        }

        [When(@"Creating a new Product with name '([^']*)' and colour '([^']*)'")]
        public async Task WhenCreatingANewProductWithNameAndColour(string aNewProduct, string colour)
        {
            var newProduct = new Product { IdKey = Guids.NewGuidString(), Name = aNewProduct, Colour = colour };
            await _productJsonDataStore.CreateProduct(newProduct);

            var products = _jsonFileService.ExtractProducts();
            _scenarioContext["Products"] = products;
        }
    
        [Then(@"Should have '([^']*)' record in the data store")]
        public void ThenShouldHaveRecordInTheDataStore(int numRecords)
        {
            var products = _scenarioContext["Products"].As<List<Product>>();
            Assert.That(products, Has.Count.EqualTo(numRecords));  
        }

        [Then(@"Should have a Product with name '([^']*)' and colour '([^']*)'")]
        public void ThenShouldHaveAProductWithNameAndColour(string aNewProduct, string colour)
        {
            var products = _scenarioContext["Products"].As<List<Product>>();
            var theProduct = products.Where(p => p.Name == aNewProduct && p.Colour.Equals(colour, StringComparison.InvariantCultureIgnoreCase)).ToList();
            Assert.That(theProduct, Has.Count.EqualTo(1));            
        }
    }
}
