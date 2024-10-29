using Products.API.Entities;
using Products.API.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.API.Specs.Helpers;

public static class ProductHelper
{
    public static IEnumerable<Product> GenerateProducts(int numberOfProducts, string colour = "Red")
    {
        List<Product> products = new();   
        for(int i = 0; i < numberOfProducts; i++)
        {
            products.Add(new Product()
            {
                IdKey = Guids.NewGuidString(),
                Name = $"Product-{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")}",
                Colour = colour,
            });
        }
        return products;
    }

}
