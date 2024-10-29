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
    Task DropDataStore_UsedForTests();
}

public class ProductJsonDataStoreService : IProductJsonDataStoreService
{
    private readonly DataStore _store;
    private readonly string _backingJsonFilename;

    public ProductJsonDataStoreService(IOptions<ApplicationSettings> options)
    {
        var filename = options.Value.DataStoreJsonFileName;
        
        _store = new DataStore(filename);   
        
        _backingJsonFilename = filename;
    }       

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
        bool isSuccess = false;
        try
        {
            var collection = _store.GetCollection<Product>();
            isSuccess = await collection.InsertOneAsync(newProduct);            
        }
        catch
        {
            throw;
        }
        if (!isSuccess)
        {
            throw new Exception($"Cannot insert product {newProduct.Name} with Id {newProduct.IdKey}");
        }
    }

    public IEnumerable<Product> GetProducts()
    {
        var collection = _store.GetCollection<Product>();
        var products = collection.AsQueryable();

        return products;
    }

    public IEnumerable<Product> GetProductsFilterColourBy(string filterColourBy) => 
        GetProducts()
            .Where(p => p.Colour.Equals(filterColourBy, StringComparison.InvariantCultureIgnoreCase));

    public async Task DropDataStore_UsedForTests()
    {
        bool isSuccess = false;
        try
        {
            _store.Reload();
            var keys = _store.GetKeys();
            foreach (var kvp in keys)
            {
                isSuccess = await _store.DeleteItemAsync(kvp.Key);
            }
            _store.Reload();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to Drop data store {ex.Message}");
        }      
    }    
}
