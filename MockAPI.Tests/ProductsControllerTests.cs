using Microsoft.AspNetCore.Mvc;
using MockApi.Api.Controllers;
using MockApi.Api.DTOs;
using MockApi.Api.Models;
using MockApi.Tests.TestData;

namespace MockApi.Tests;

public class ProductsControllerTests
{
    private static ProductsController CreateController(
        IEnumerable<Product>? products = null,
        IEnumerable<Category>? categories = null)
    {
        var productRepository = new TestDataProductRepository(products);
        var categoryRepository = new TestDataCategoryRepository(categories);
        return new ProductsController(productRepository, categoryRepository);
    }

    [Fact]
    public void GetFiltered_NoFilter_ReturnsAllProducts()
    {
        var controller = CreateController();

        var result = controller.GetFiltered(new ProductFilterQuery());

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var products = Assert.IsAssignableFrom<IEnumerable<ProductResponse>>(okResult.Value);
        Assert.Equal(2, products.Count());
    }
}
