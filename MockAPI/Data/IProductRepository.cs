using MockApi.Api.DTOs;
using MockApi.Api.Models;

namespace MockApi.Api.Data;

public interface IProductRepository
{
    IEnumerable<Product> GetFiltered(ProductFilterQuery filter);

    Product? GetById(int id);

    Product? Update(int id, UpdateProductRequest request);
}
