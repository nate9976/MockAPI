using MockApi.Api.Validation;
using System.ComponentModel.DataAnnotations;

namespace MockApi.Api.DTOs;

[AtLeastOneFieldRequired(nameof(Name), nameof(Price), ErrorMessage = "Provide at least a Name or a Price to update.")]
public class UpdateProductRequest
{
    [MinLength(1, ErrorMessage = "Name cannot be empty.")]
    [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
    public string? Name { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal? Price { get; set; }
}
