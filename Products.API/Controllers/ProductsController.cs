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
    private readonly ILogger<ProductsController> _logger;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductsController(
        ILogger<ProductsController> logger,
        IProductRepository productRepository, 
        IMapper mapper)
    {
        _logger = logger;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    [HttpGet("ping")]
    public string Index() => "ProductsController OK";
    
    [HttpPost("CreateProduct", Name = "CreateProduct")]
    public ActionResult<ProductDto> CreateProduct(ProductForCreationDto productForCreationDto)
    {        
        try
        {            
            var productEntity = _mapper.Map(productForCreationDto);

            _productRepository.AddProduct(productEntity);

            var newProductDto = _mapper.Map(productEntity);

            return Ok(newProductDto);
        }
        catch (Exception ex)
        {
            _logger.LogError($"CreateProduct API error, params Name:{productForCreationDto.Name}, Colour:{productForCreationDto.Colour}, Reason: '{ex.Message}'");
            throw;
        }
    }

    [HttpGet(Name="GetProducts")]
    public ActionResult<List<ProductDto>> GetProducts([FromQuery] GetProductsParameters parameters)
    {
        try
        {
            var products = _productRepository.GetProducts(parameters.FilterColourBy);

            if (!parameters.FilterColourBy.IsNullOrEmpty() && !products.Any())
            {
                return NotFound($"FilterColourBy: {parameters.FilterColourBy} returned no elements.");
            }

            IEnumerable<ProductDto> productDtos = Enumerable.Empty<ProductDto>();
            productDtos = products.Select(p => _mapper.Map(p));

            return Ok(productDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"GetProducts API call error, params: {nameof(parameters.FilterColourBy)} {parameters.FilterColourBy}, Reason:{ex.Message}");
            throw;
        }        
    }
}
