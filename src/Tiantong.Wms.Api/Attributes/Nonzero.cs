using System.ComponentModel.DataAnnotations;

namespace Tiantong.Wms.Api
{
  public class NonzeroAttribute : ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      if ((int) value == 0) {
        return new ValidationResult("Zero value is invalid");
      }

      return ValidationResult.Success;
    }
  }
}
