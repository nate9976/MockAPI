using System.Collections.Concurrent;
using MockApi.Api.DTOs;
using MockApi.Api.Models;

namespace MockApi.Api.Data;

public class InMemoryProductRepository : IProductRepository
{
    private readonly ConcurrentDictionary<int, Product> _products;
    private readonly ICategoryRepository _categoryRepository;
    private readonly object _writeLock = new();

    public InMemoryProductRepository(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
        _products = new ConcurrentDictionary<int, Product>(
            SeedProducts().ToDictionary(p => p.Id));
    }

    public IEnumerable<Product> GetFiltered(ProductFilterQuery filter)
    {
        var query = _products.Values.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(p => p.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(filter.CategoryName))
        {
            var matchingCategoryIds = _categoryRepository.GetAll()
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

        return query.OrderBy(p => p.Id).ToList();
    }

    public Product? GetById(int id)
    {
        return _products.TryGetValue(id, out var product) ? product : null;
    }

    public Product? Update(int id, UpdateProductRequest request)
    {
        lock (_writeLock)
        {
            if (!_products.TryGetValue(id, out var existing))
            {
                return null;
            }

            var updated = new Product
            {
                Id = existing.Id,
                CategoryId = existing.CategoryId,
                Name = request.Name ?? existing.Name,
                Price = request.Price ?? existing.Price,
            };

            _products[id] = updated;
            return updated;
        }
    }

    private static IEnumerable<Product> SeedProducts()
    {
        return new List<Product>
        {
            new() { Id = 1, Name = "Wireless Mouse", Price = 19.90m, CategoryId = 1 },
            new() { Id = 2, Name = "Mechanical Keyboard", Price = 79.80m, CategoryId = 1 },
            new() { Id = 3, Name = "USB-C Hub", Price = 34.50m, CategoryId = 1 },
            new() { Id = 4, Name = "Novel: The Great Adventure", Price = 12.10m, CategoryId = 2 },
            new() { Id = 5, Name = "Cookbook: Easy Dinners", Price = 24.00m, CategoryId = 2 },
            new() { Id = 6, Name = "Running Shoes", Price = 89.99m, CategoryId = 3 },
            new() { Id = 7, Name = "Yoga Mat", Price = 29.98m, CategoryId = 3 },
        };
    }
}
