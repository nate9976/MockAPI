using MockApi.Api.Data;
using MockApi.Api.Models;

namespace MockApi.Tests.TestData;

public class TestDataCategoryRepository : ICategoryRepository
{
    private readonly List<Category> _categories;

    public TestDataCategoryRepository(IEnumerable<Category>? categories = null)
    {
        _categories = categories?.ToList() ?? new List<Category>
        {
            new() { Id = 1, Name = "Widgets" },
            new() { Id = 2, Name = "Electronics" },
        };
    }

    public IEnumerable<Category> GetAll() => _categories;

    public Category? GetById(int id) => _categories.FirstOrDefault(c => c.Id == id);
}
