using Microsoft.AspNetCore.Mvc;
using MockApi.Api.Data;
using MockApi.Api.DTOs;

namespace MockApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProductResponse>> GetFiltered([FromQuery] ProductFilterQuery filter)
    {
        var products = _productRepository.GetFiltered(filter);
        return Ok(products);
    }

    [HttpPatch("{id:int}")]
    public ActionResult<ProductResponse> UpdateProduct(int id, [FromBody] UpdateProductRequest request)
    {
        var updated = _productRepository.Update(id, request);

        if (updated is null)
        {
            return NotFound(new { message = $"Product with id {id} was not found." });
        }

        return Ok(updated);
    }
}
