using System.ComponentModel.DataAnnotations;

namespace Tiantong.Iot.Entities
{
  public class PlcStateTypeAttribute: ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
      if (PlcStateType.IsValid(value.ToString())) {
        return new ValidationResult("PLC 数据类型错误");
      } else {
        return ValidationResult.Success;
      }
    }
  } 
}