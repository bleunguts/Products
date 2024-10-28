using JsonFlatFileDataStore;
using Microsoft.Extensions.Options;
using Products.API.Entities;
using Products.API.Utils;

namespace Products.API.Services;

public interface IProductJsonDataStoreService
{
    Task CreateProduct(Product newProduct);
    Task CreateProducts(IEnumerable<Product> someProducts);
    IEnumerable<Product> GetProducts();
    IEnumerable<Product> GetProductsFilterColourBy(string filterColourBy);
    string BackingJsonFilename { get; }
    void Drop();
}

public class ProductJsonDataStoreService : IProductJsonDataStoreService
{
    private readonly DataStore _store;

    public ProductJsonDataStoreService(IOptions<ApplicationSettings> options)
    {
        var filename = options.Value.DataStoreJsonFileName;
        
        _store = new DataStore(filename);   
        
        BackingJsonFilename = filename;
    }
    
    public string BackingJsonFilename { get; }

    public async Task CreateProducts(IEnumerable<Product> products)
    {
        var collection = _store.GetCollection<Product>();        
        if (await collection.InsertManyAsync(products))
        {
            throw new Exception($"Cannot insert collection of {products.Count()} products.");
        }    
    }

    public async Task CreateProduct(Product newProduct)
    {
        var collection = _store.GetCollection<Product>();
        if (!await collection.InsertOneAsync(newProduct))
        {
            throw new Exception($"Cannot insert product {newProduct.Name} with Id {newProduct.Id}");
        };        
    }

    public IEnumerable<Product> GetProducts()
    {
        var collection = _store.GetCollection<Product>();
        var products = collection.AsQueryable();

        return products;
    }

    public IEnumerable<Product> GetProductsFilterColourBy(string filterColourBy)
    {
        var collection = _store.GetCollection<Product>();
        var products = collection.AsQueryable().Where(p => p.Colour.Equals(filterColourBy, StringComparison.InvariantCultureIgnoreCase));

        return products;
    }

    public void Drop()
    {
        FileHelper.ForceDelete(BackingJsonFilename);
        _store.Reload();
    }    
}
