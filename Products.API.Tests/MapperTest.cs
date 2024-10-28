using Products.API.Services;
using Products.API.Utils;
using Products.Model;
using System.Drawing;

namespace Products.API.Tests;

public class MapperTests
{       
    private Mapper _mapper = new Mapper();
    [Test]
    public void WhenMappingProductDtoToProductEntityShouldCreateMatchingEntityFields()
    {
        var actual = _mapper.Map(new Model.ProductForCreationDto { Name = "aProduct", Colour = "aNamedColour" });
        Assert.That(actual.Id, Is.EqualTo("AAAAAAAAAAAAAAAAAAAAAA"));
        Assert.That(actual.Name, Is.EqualTo("aProduct"));
        Assert.That(actual.Colour, Is.EqualTo("aNamedColour"));
    }

    [Test]
    public void WhenMappingProductEntityBackToProductDtoShouldReturnMatchingFields()
    {
        var newId = ShortGuid.NewGuid();
        var actual = _mapper.Map(new Entities.Product { Id = newId, Name = "aProduct", Colour = "Black" });
        Assert.That(actual, Is.InstanceOf<ProductDto>());
        Assert.That(actual.Name, Is.EqualTo("aProduct"));
        Assert.That(actual.Colour, Is.EqualTo("Black"));
        Assert.That(actual.Id, Is.EqualTo(newId));
    }  
}