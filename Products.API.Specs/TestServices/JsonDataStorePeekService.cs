using Products.API.Entities;
using SpecFlow.Internal.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;

namespace Products.API.Specs.Helpers;

public class JsonDataStorePeekService
{
    private readonly string _filename;    

    public JsonDataStorePeekService(string filename)
    {
        _filename = filename;        
    }

    public List<Product> ExtractProducts()
    {
        var json = File.ReadAllText(_filename);        
        var productJObject = JsonNode.Parse(json).AsObject();
        var productsJArray = productJObject["product"];
        return productsJArray.Deserialize<List<Product>>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });        
    }
}
