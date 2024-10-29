using Products.API.Entities;
using Products.Model;
using System.Drawing;

namespace Products.API.Services;
public interface IMapper
{
    Product Map(ProductForCreationDto productForCreationDto);
    ProductDto Map(Product productEntity);    
}

/// <summary>
/// Maps Dto to Entity.Model and vice versa
/// lighter version of AutoMapper
/// </summary>
public class Mapper : IMapper
{
    public Product Map(ProductForCreationDto productForCreationDto) => new()
    {
        Name = productForCreationDto.Name,
        Colour = productForCreationDto.Colour != null ? productForCreationDto.Colour.Value.Name : string.Empty,
    };

    public ProductDto Map(Product productEntity)
    {
        if (!Guid.TryParse(productEntity.IdKey, out var guid))
        {
            throw new Exception($"Cannot parse IdKey field to a guid, Length:{productEntity.IdKey.Count()}, {productEntity.Name}");
        }
        return new()
        {
            Id = guid,
            Name = productEntity.Name,
            Colour = Color.FromName(productEntity.Colour),
        };
    }
}