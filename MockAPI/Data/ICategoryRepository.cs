using MockApi.Api.Models;

namespace MockApi.Api.Data;

public interface ICategoryRepository
{
    IEnumerable<Category> GetAll();

    Category? GetById(int id);
}
