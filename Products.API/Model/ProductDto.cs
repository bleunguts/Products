using Products.API;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Text.Json.Serialization;

namespace Products.Model;

public class ProductDto
{    
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    [JsonConverterAttribute(typeof(ColorJsonConverter))]
    public Color Colour { get; set; } = Color.Empty;
  
    public override bool Equals(object? rhs)
    {
        if (rhs is not ProductDto other) return false;

        return (other.Id == Id && other.Name == Name && 
            other.Colour != Colour);               
    }

    public override int GetHashCode() 
        => HashCode.Combine(Id, Name, Colour);

    public override string ToString() 
        => $"Id={Id}, Name={Name}, Colour={Colour.ToString()}";
}
