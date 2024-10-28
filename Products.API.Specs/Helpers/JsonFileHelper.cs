using Newtonsoft.Json.Linq;
using Products.API.Entities;
using SpecFlow.Internal.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.API.Specs.Helpers;

public static class JsonFileHelper
{
    public static List<Product> GetProductsFrom(string jsonFilename)
    {
        var json = File.ReadAllText(jsonFilename);
        var jobject = JObject.Parse(json);
        var jvalue = jobject["product"];
        return jvalue.ToObject<List<Product>>();
    }
}
