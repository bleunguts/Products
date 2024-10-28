using System.ComponentModel.DataAnnotations;

namespace Products.Model;
public class ProductForCreationDto
{
    [Required]    
    public string Name { get; set; } = string.Empty;

    public string Colour { get; set; } =  string.Empty;
}
