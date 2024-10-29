using Newtonsoft.Json.Linq;
using Products.API.Entities;
using Products.API.Utils;
using Products.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Products.API.Tests;

public class ProductDtoSerDerRoundTripTest
{
    private static List<ProductDto> GenerateProducts(int num)
    {
        var productDtos = new List<ProductDto>();
        for (int i = 0; i < num; i++)
        {
            productDtos.Add(GenerateProductDto());
        }
        return productDtos;
    }
    private static ProductDto GenerateProductDto() =>
        new()
        { Id = Guid.NewGuid(), Colour = Color.Black, Name = "MyProduct_" + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")};

    [Test]
    public void RoundTripAProductDto()
    {
        ProductDto dto = GenerateProductDto();
        var json = System.Text.Json.JsonSerializer.Serialize(dto);
        Assert.That(json, Is.Not.Null.Or.Empty);

        var dtoActual = System.Text.Json.JsonSerializer.Deserialize<ProductDto>(json);
        Assert.That(dtoActual, Is.EqualTo(dto));
    }

    [Test]
    public void RoundTripACollectionOfProductDtos()
    {
        int num = 5;
        var productDtos = GenerateProducts(num);                    
        var json = System.Text.Json.JsonSerializer.Serialize(productDtos);        
        var dtosActual = System.Text.Json.JsonSerializer.Deserialize<List<ProductDto>>(json);

        Assert.That(dtosActual, Is.Not.Null);
        Assert.That(dtosActual, Has.Count.EqualTo(num));
        CollectionAssert.AreEquivalent(dtosActual, productDtos);
    }
}
