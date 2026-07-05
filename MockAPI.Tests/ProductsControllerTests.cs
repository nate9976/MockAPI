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

    [Fact]
    public void GetFiltered_ReturnsCategoryNameInsteadOfCategoryId()
    {
        var controller = CreateController();

        var result = controller.GetFiltered(new ProductFilterQuery { Name = "widget" });

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var products = Assert.IsAssignableFrom<IEnumerable<ProductResponse>>(okResult.Value).ToList();
        var product = Assert.Single(products);
        Assert.Equal("Test Widget", product.Name);
        Assert.Equal("Widgets", product.CategoryName);
    }

    [Fact]
    public void GetFiltered_ByName_ReturnsMatchingProductsOnly()
    {
        var controller = CreateController();

        var result = controller.GetFiltered(new ProductFilterQuery { Name = "widget" });

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var products = Assert.IsAssignableFrom<IEnumerable<ProductResponse>>(okResult.Value).ToList();
        var product = Assert.Single(products);
        Assert.Equal("Test Widget", product.Name);
    }

    [Fact]
    public void GetFiltered_ByCategoryName_ReturnsMatchingProductsOnly()
    {
        var controller = CreateController();

        var result = controller.GetFiltered(new ProductFilterQuery { CategoryName = "Electronics" });

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var products = Assert.IsAssignableFrom<IEnumerable<ProductResponse>>(okResult.Value).ToList();
        var product = Assert.Single(products);
        Assert.Equal("Test Gadget", product.Name);
        Assert.Equal("Electronics", product.CategoryName);
    }

    [Fact]
    public void GetFiltered_ByPriceRange_ReturnsProductsWithinRangeOnly()
    {
        var controller = CreateController();

        var result = controller.GetFiltered(new ProductFilterQuery { MinPrice = 10m, MaxPrice = 100m });

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var products = Assert.IsAssignableFrom<IEnumerable<ProductResponse>>(okResult.Value).ToList();
        var product = Assert.Single(products);
        Assert.Equal("Test Gadget", product.Name);
    }

    [Fact]
    public void GetFiltered_CombinedFilters_NarrowsToExpectedProduct()
    {
        var controller = CreateController();

        var result = controller.GetFiltered(new ProductFilterQuery { Name = "test", CategoryName = "Widgets", MaxPrice = 20m });

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var products = Assert.IsAssignableFrom<IEnumerable<ProductResponse>>(okResult.Value).ToList();
        var product = Assert.Single(products);
        Assert.Equal("Test Widget", product.Name);
    }

    [Fact]
    public void GetFiltered_NoMatches_ReturnsEmptyCollection()
    {
        var controller = CreateController();

        var result = controller.GetFiltered(new ProductFilterQuery { Name = "does-not-exist" });

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var products = Assert.IsAssignableFrom<IEnumerable<ProductResponse>>(okResult.Value);
        Assert.Empty(products);
    }

    [Fact]
    public void GetFiltered_UnknownCategoryIdOnProduct_ReturnsUnknownCategoryName()
    {
        var orphanProduct = new List<Product>
        {
            new() { Id = 1, Name = "Orphan Product", Price = 5.00m, CategoryId = 999 },
        };
        var controller = CreateController(orphanProduct);

        var result = controller.GetFiltered(new ProductFilterQuery());

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var products = Assert.IsAssignableFrom<IEnumerable<ProductResponse>>(okResult.Value).ToList();
        var product = Assert.Single(products);
        Assert.Equal("Unknown", product.CategoryName);
    }

    [Fact]
    public void UpdateProduct_ExistingId_UpdatesNameAndPriceAndReturnsCategoryName()
    {
        var controller = CreateController();

        var result = controller.UpdateProduct(1, new UpdateProductRequest { Name = "Updated Widget", Price = 15.00m });

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var product = Assert.IsType<ProductResponse>(okResult.Value);
        Assert.Equal("Updated Widget", product.Name);
        Assert.Equal(15.00m, product.Price);
        Assert.Equal("Widgets", product.CategoryName);
    }

    [Fact]
    public void UpdateProduct_OnlyPriceSupplied_LeavesNameUnchanged()
    {
        var controller = CreateController();

        var result = controller.UpdateProduct(1, new UpdateProductRequest { Price = 25.00m });

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var product = Assert.IsType<ProductResponse>(okResult.Value);
        Assert.Equal("Test Widget", product.Name);
        Assert.Equal(25.00m, product.Price);
    }

    [Fact]
    public void UpdateProduct_UnknownId_ReturnsNotFound()
    {
        var controller = CreateController();

        var result = controller.UpdateProduct(999, new UpdateProductRequest { Name = "Doesn't matter" });

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}
