using MockApi.Api.Data;
using MockApi.Api.DTOs;
using MockApi.Api.Models;

namespace MockApi.Tests.TestData;

public class TestDataProductRepository : IProductRepository
{
    private readonly List<Product> _products;
    private readonly List<Category> _categories;

    public TestDataProductRepository(IEnumerable<Product>? products = null, IEnumerable<Category>? categories = null)
    {
        _products = products?.ToList() ?? new List<Product>
        {
            new() { Id = 1, Name = "Test Widget", Price = 9.99m, CategoryId = 1 },
            new() { Id = 2, Name = "Test Gadget", Price = 49.99m, CategoryId = 2 },
        };

        _categories = categories?.ToList() ?? new List<Category>
        {
            new() { Id = 1, Name = "Widgets" },
            new() { Id = 2, Name = "Electronics" },
        };
    }

    public IEnumerable<Product> GetFiltered(ProductFilterQuery filter)
    {
        var query = _products.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(p => p.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(filter.CategoryName))
        {
            var matchingCategoryIds = _categories
                .Where(c => c.Name.Contains(filter.CategoryName, StringComparison.OrdinalIgnoreCase))
                .Select(c => c.Id)
                .ToHashSet();

            query = query.Where(p => matchingCategoryIds.Contains(p.CategoryId));
        }

        if (filter.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= filter.MinPrice.Value);
        }

        if (filter.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= filter.MaxPrice.Value);
        }

        return query.ToList();
    }

    public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

    public Product? Update(int id, UpdateProductRequest request)
    {
        var existing = _products.FirstOrDefault(p => p.Id == id);
        if (existing is null)
        {
            return null;
        }

        if (request.Name is not null)
        {
            existing.Name = request.Name;
        }

        if (request.Price.HasValue)
        {
            existing.Price = request.Price.Value;
        }

        return existing;
    }
}
