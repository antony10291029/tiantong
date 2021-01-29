using System.Text.Json;
using System.Net;
using System.ComponentModel.DataAnnotations;

namespace Renet.Web.Attributes
{
  public class JsonObjectAttribute: ValidationAttribute
  {
    public JsonObjectAttribute()
    {
      ErrorMessage = "JSON 数据格式错误";
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      try {
        var dom = JsonDocument.Parse(value.ToString());
        var obj = dom.RootElement.EnumerateObject();

        return ValidationResult.Success;
      } catch {
        return new ValidationResult(ErrorMessage);
      }
    }
  }
}