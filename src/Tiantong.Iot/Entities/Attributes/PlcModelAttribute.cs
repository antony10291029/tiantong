using System.ComponentModel.DataAnnotations;

namespace Tiantong.Iot.Entities
{
  public class PlcModelAttribute: ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
      if (PlcModel.IsValid(value.ToString())) {
        return new ValidationResult("暂时不支持该 PLC 型号");
      } else {
        return ValidationResult.Success;
      }
    }
  } 
}