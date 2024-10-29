using Products.API.Utils;

namespace Products.API.Entities;

public class Product
{    
    // Note that 'Id' clashes with JsonDataStoreService so call it something else
    public string IdKey { get; set; } = Guids.NewGuidString();
    public string Name { get; set; } = string.Empty;
    public string Colour { get; set; } = string.Empty;
}
