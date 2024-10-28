using Products.API.Entities;
using Products.Model;

namespace Products.API.Services;
public interface IMapper
{
    Product Map(ProductForCreationDto productForCreationDto);
    ProductDto Map(Product productEntity);    
}
public class Mapper : IMapper
{
    public Product Map(ProductForCreationDto productForCreationDto) => new()
    {
        Name = productForCreationDto.Name,
        Colour = productForCreationDto.Colour,
    };

    public ProductDto Map(Product productEntity) => new()
    {
        Id = productEntity.Id,
        Name = productEntity.Name,
        Colour = productEntity.Colour,
    };
}