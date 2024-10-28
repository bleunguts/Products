using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Products.API.Entities;
using Products.API.ResourceParameters;
using Products.API.Services;
using Products.Model;

namespace Products.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductsController(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    [HttpGet("ping")]
    public string Index() => "ProductsController OK";

    [HttpPost("CreateProduct", Name = "CreateProduct")]
    public ActionResult<ProductDto> CreateProduct(ProductForCreationDto productForCreationDto)
    {
        if (!ModelState.IsValid)
        {
            //  TODO: log warning 
            return BadRequest(ModelState);
        }

        var productEntity = _mapper.Map(productForCreationDto);
        _productRepository.AddProduct(productEntity);

        var newProductDto = _mapper.Map(productEntity);

        return Ok(newProductDto);
    }

    [HttpGet(Name="GetProducts")]
    public ActionResult<List<ProductDto>> GetProducts([FromQuery] GetProductsParameters parameters)
    {
        if (!ModelState.IsValid)
        {
            //  TODO: log warning 
            return BadRequest(ModelState);
        }

        var products = _productRepository.GetProducts(parameters.FilterColourBy);
        
        if(!parameters.FilterColourBy.IsNullOrEmpty() && !products.Any())
        {            
            return NotFound($"FilterColourBy: {parameters.FilterColourBy} returned no elements.");
        }
        
        var productDtos = products.Select(p => _mapper.Map(p));      

        return Ok(productDtos);  
    }
}
