using Moq;
using Products.API.Entities;
using Products.API.Services;
using Products.API.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.API.Tests;

public class ProductRepositoryTest
{
    private Mock<IProductJsonDataStoreService> _dataStore = new Mock<IProductJsonDataStoreService>();
    private ProductRepository _repository;
    
    [SetUp]
    public void Setup()
    {
        _repository = new ProductRepository(_dataStore.Object);
    }

    [Test]
    public void WhenAddingAProductItShouldAddItToBackingStore()
    {        
        // arrange
        var theName = "aNewProduct";
        var theColour = "Black";
        Product? actualProduct = null;
        _dataStore.Setup(d => d.CreateProduct(It.IsAny<Product>()))
                     .Callback<Product>((obj) => actualProduct = obj);

        // act
        _repository.AddProduct(new Entities.Product { Id = Guid.Empty.ToString().Replace("-",""), Name = theName, Colour = theColour });
        
        // assert
        _dataStore.Verify(d => d.CreateProduct(It.IsAny<Product>()), Times.Once);
        Assert.That(actualProduct.Id, Is.Not.EqualTo(Guid.Empty.ToString().Replace("-","")));
        Assert.That(actualProduct.Name, Is.EqualTo(theName));
        Assert.That(actualProduct.Colour, Is.EqualTo(theColour));
    }

    [Test]
    public void WhenGettingProductsItShouldInvokeBackingStore()
    {
        // arrange
        var expectedProducts = new[] 
        { 
            new Product { Name = "aProduct1"}, 
            new Product { Name = "aProduct2", Colour = "Grey"}, 
            new Product { Name = "aProduct3", Colour = "Purple"} 
        };
        _dataStore.Setup(d => d.GetProducts()).Returns(expectedProducts);
        // act
        var products = _repository.GetProducts();

        // assert
        CollectionAssert.AreEqual(products, expectedProducts);                
    }
}
