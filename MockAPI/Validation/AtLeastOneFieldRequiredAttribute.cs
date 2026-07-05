using System.ComponentModel.DataAnnotations;

namespace MockApi.Api.Validation;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class AtLeastOneFieldRequiredAttribute : ValidationAttribute
{
    private readonly string[] _propertyNames;

    public AtLeastOneFieldRequiredAttribute(params string[] propertyNames)
    {
        _propertyNames = propertyNames;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        if (_propertyNames is null || _propertyNames.Length == 0)
        {
            return new ValidationResult(ErrorMessage, _propertyNames);
        }

        var type = value.GetType();

        var hasAnyValue = _propertyNames.Any(propertyName =>
        {
            var property = type.GetProperty(propertyName);
            return property is not null && property.GetValue(value) is not null;
        });

        if (hasAnyValue)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(ErrorMessage, _propertyNames);
    }
}
