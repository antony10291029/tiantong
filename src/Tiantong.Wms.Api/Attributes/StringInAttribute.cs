using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Tiantong.Wms.Api
{
  public class StringInAttribute : ValidationAttribute
  {
    private object[] _values;

    public StringInAttribute(params string[] values)
    {
      _values = values;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      if (!_values.Any(val => val.Equals(value))) {
        return new ValidationResult(ErrorMessage);
      } else {
        return ValidationResult.Success;
      }
    }
  }
}
