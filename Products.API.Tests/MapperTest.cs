using Products.API.Services;
using Products.API.Utils;
using Products.Model;
using System;
using System.Drawing;

namespace Products.API.Tests;

public class MapperTests
{       
    private Mapper _mapper = new Mapper();
    [Test]
    public void WhenMappingProductDtoToProductEntityShouldCreateMatchingEntityFields()
    {
        var actual = _mapper.Map(new Model.ProductForCreationDto { Name = "aProduct", Colour = Color.Aqua });
        Assert.That(actual.IdKey, Is.Not.EqualTo(Guid.Empty.ToGuidString()));
        Assert.That(actual.Name, Is.EqualTo("aProduct"));
        Assert.That(actual.Colour, Is.EqualTo("Aqua"));
    }

    [Test]
    public void WhenMappingProductEntityBackToProductDtoShouldReturnMatchingFields()
    {
        var newId = Guids.NewGuidString();
        var actual = _mapper.Map(new Entities.Product { IdKey = newId, Name = "aProduct", Colour = "Black" });
        Assert.That(actual, Is.InstanceOf<ProductDto>());
        Assert.That(actual.Name, Is.EqualTo("aProduct"));
        Assert.That(actual.Colour, Is.EqualTo(Color.Black));
        Assert.That(actual.Id.ToGuidString(), Is.EqualTo(newId));
    }  
}