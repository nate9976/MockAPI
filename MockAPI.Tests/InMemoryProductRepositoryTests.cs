using MockApi.Api.Data;
using MockApi.Api.DTOs;
using Xunit;

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
}
