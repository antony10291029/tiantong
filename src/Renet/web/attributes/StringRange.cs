using System.ComponentModel.DataAnnotations;

namespace Renet.Web.Attributes
{
  public class StringRangeAttribute: ValidationAttribute
  {
    private int _min;

    private int _max;

    public StringRangeAttribute(int min, int max)
    {
      _min = min;
      _max = max;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      var length = value.ToString().Length;

      if (length > _min && length < _max) {
        return ValidationResult.Success;
      } else {
        return new ValidationResult(ErrorMessage);
      }
    }
  }
}
