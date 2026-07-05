using System.Collections.Concurrent;
using MockApi.Api.Models;

namespace MockApi.Api.Data;

public class InMemoryCategoryRepository : ICategoryRepository
{
    private readonly ConcurrentDictionary<int, Category> _categories;

    public InMemoryCategoryRepository()
    {
        _categories = new ConcurrentDictionary<int, Category>(
            MockCategories().ToDictionary(c => c.Id));
    }

    public IEnumerable<Category> GetAll() => _categories.Values.OrderBy(c => c.Id).ToList();

    public Category? GetById(int id) => _categories.TryGetValue(id, out var category) ? category : null;

    private static IEnumerable<Category> MockCategories()
    {
        return new List<Category>
        {
            new() { Id = 1, Name = "Electronics", Description = "Gadgets and accessories." },
            new() { Id = 2, Name = "Books", Description = "Fiction and non-fiction titles." },
            new() { Id = 3, Name = "Sports", Description = "Gear and apparel for an active lifestyle." },
        };
    }
}
