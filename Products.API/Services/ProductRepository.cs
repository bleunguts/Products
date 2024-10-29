using Microsoft.AspNetCore.Mvc;
using Products.API.Entities;
using Products.API.Utils;

namespace Products.API.Services;

public interface IProductRepository
{
    void AddProduct(Product productEntity);
    IEnumerable<Product> GetProducts(string? filterColourBy = null);
}

public class ProductRepository : IProductRepository
{
    private readonly IProductJsonDataStoreService _dataStore;

    public ProductRepository(IProductJsonDataStoreService dataStore)
    {
        _dataStore = dataStore;
    }
    public void AddProduct(Product productEntity)
    {
        if(productEntity == null) throw new ArgumentNullException(nameof(productEntity));

        productEntity.IdKey = Guids.NewGuidString();

        _dataStore.CreateProduct(productEntity);    
    }

    public IEnumerable<Product> GetProducts(string? filterColourBy = null)
    {
        if (!string.IsNullOrEmpty(filterColourBy))
        {
            return _dataStore.GetProductsFilterColourBy(filterColourBy);
        }        
        
        return _dataStore.GetProducts();       
    }
}

