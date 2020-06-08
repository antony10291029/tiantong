using System.Net;
using System.ComponentModel.DataAnnotations;

namespace Renet.Web.Attribute
{
  public class IPPortAttribute: ValidationAttribute
  {
    public IPPortAttribute()
    {
      ErrorMessage = "端口范围必须在 1 至 65535";
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      if (IPAddress.TryParse(value.ToString(), out _)) {
        return ValidationResult.Success;
      } else {
        return new ValidationResult(ErrorMessage);
      }
    }
  }
}