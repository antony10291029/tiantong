using System.ComponentModel.DataAnnotations;

namespace Tiantong.Wms.Api
{
  public class StringRangeAttribute : ValidationAttribute
  {
    private int _minimum;

    private int _maximum;

    public StringRangeAttribute(int minimum, int maximum)
    {
      _minimum = minimum;
      _maximum = maximum;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      if (value == null) {
        return ValidationResult.Success;
      }

      var length = value.ToString().Length;

      if (length < _minimum || length > _maximum) {
        return new ValidationResult(ErrorMessage);
      }

      return ValidationResult.Success;
    }
  }
}
