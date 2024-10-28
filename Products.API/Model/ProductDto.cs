using Products.API;

namespace Products.Model;

public class ProductDto
{    
    public string Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Colour { get; set; } = string.Empty;
}
