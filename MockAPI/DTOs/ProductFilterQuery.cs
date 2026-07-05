using System.ComponentModel.DataAnnotations;

namespace MockApi.Api.DTOs;

public class ProductFilterQuery : IValidatableObject
{
    public string? Name { get; set; }

    public string? CategoryName { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "MinPrice must be zero or greater.")]
    public decimal? MinPrice { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "MaxPrice must be zero or greater.")]
    public decimal? MaxPrice { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MinPrice.HasValue && MaxPrice.HasValue && MinPrice > MaxPrice)
        {
            yield return new ValidationResult(
                "MinPrice cannot be greater than MaxPrice.",
                new[] { nameof(MinPrice), nameof(MaxPrice) });
        }
    }
}
