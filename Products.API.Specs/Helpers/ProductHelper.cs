using Products.API.Entities;
using Products.API.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.API.Specs.Helpers;

public static class ProductHelper
{
    public static IEnumerable<Product> GenerateProducts(int numberOfProducts, string colour = "Red") =>
        Enumerable.Range(0, numberOfProducts)
                    .Select(i => new Product
                    {
                        Id = ShortGuid.NewGuid(),
                        Name = $"Product-{i}",
                        Colour = colour,
                    });

}
