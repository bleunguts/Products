using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace Products.Model;
public class ProductForCreationDto
{
    [Required]    
    public string Name { get; set; } = string.Empty;   
    public Color? Colour { get; set; } = null;
}
