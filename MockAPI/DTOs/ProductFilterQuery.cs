using System.ComponentModel.DataAnnotations;

namespace MockApi.Api.DTOs;

public class ProductFilterQuery
{
    public string? Name { get; set; }
    public string? CategoryName { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}
