using Products.API.Utils;

namespace Products.API.Entities;

public class Product
{    
    public string Id { get; set; } = ShortGuid.GuidToShortGuid(Guid.Empty);
    public string Name { get; set; } = string.Empty;
    public string Colour { get; set; } = string.Empty;
}
