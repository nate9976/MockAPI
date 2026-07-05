using MockApi.Api.Data;
using MockApi.Api.DTOs;

namespace MockApi.Tests;

public class InMemoryProductRepositoryTests
{
    private static InMemoryProductRepository CreateRepository()
    {
        return new InMemoryProductRepository(new InMemoryCategoryRepository());
    }

    [Fact]
    public void GetFiltered_ByCategoryName_ReturnsOnlyThatCategory()
    {
        var repository = CreateRepository();

        var result = repository.GetFiltered(new ProductFilterQuery { CategoryName = "Books" }).ToList();

        Assert.NotEmpty(result);
        Assert.All(result, p => Assert.Equal(2, p.CategoryId));
    }

    [Fact]
    public void GetFiltered_ByCategoryNamePartialMatch_IsCaseInsensitive()
    {
        var repository = CreateRepository();

        var result = repository.GetFiltered(new ProductFilterQuery { CategoryName = "elect" }).ToList();

        Assert.NotEmpty(result);
        Assert.All(result, p => Assert.Equal(1, p.CategoryId));
    }

    [Fact]
    public void GetFiltered_CombinedFilters_NarrowsResults()
    {
        var repository = CreateRepository();

        var result = repository.GetFiltered(new ProductFilterQuery
        {
            CategoryName = "Electronics",
            MinPrice = 20m,
            MaxPrice = 50m,
        }).ToList();

        Assert.NotEmpty(result);
        Assert.All(result, p =>
        {
            Assert.Equal(1, p.CategoryId);
            Assert.InRange(p.Price, 20m, 50m);
        });
    }

    [Fact]
    public void GetFiltered_ByNamePartialMatch_IsCaseInsensitive()
    {
        var repository = CreateRepository();

        var result = repository.GetFiltered(new ProductFilterQuery { Name = "MOUSE" }).ToList();

        var product = Assert.Single(result);
        Assert.Equal("Wireless Mouse", product.Name);
    }

    [Fact]
    public void Update_ExistingProduct_ChangesOnlyProvidedFields()
    {
        var repository = CreateRepository();
        var original = repository.GetById(1);
        Assert.NotNull(original);

        var updated = repository.Update(1, new UpdateProductRequest { Price = 99.99m });

        Assert.NotNull(updated);
        Assert.Equal(original!.Name, updated!.Name);
        Assert.Equal(99.99m, updated.Price);
    }

    [Fact]
    public void Update_UnknownProduct_ReturnsNull()
    {
        var repository = CreateRepository();

        var updated = repository.Update(9999, new UpdateProductRequest { Name = "Ghost" });

        Assert.Null(updated);
    }
}
