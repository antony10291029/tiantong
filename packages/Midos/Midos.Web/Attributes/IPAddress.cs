using System.Net;
using System.ComponentModel.DataAnnotations;

namespace Midos.Web
{
  public class IPAddressAttribute: ValidationAttribute
  {
    public IPAddressAttribute()
    {
      ErrorMessage = "IP 地址格式错误";
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      if (value.ToString() == "localhost" || IPAddress.TryParse(value.ToString(), out _)) {
        return ValidationResult.Success;
      } else {
        return new ValidationResult(ErrorMessage);
      }
    }
  }
}
