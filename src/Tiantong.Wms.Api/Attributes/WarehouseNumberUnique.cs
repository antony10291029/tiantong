using System.ComponentModel.DataAnnotations;

namespace Tiantong.Wms.Api
{
  public class WarehouseNumberUniqueAttribute : ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      var auth = validationContext.GetService(typeof(IAuth)) as IAuth;
      var warehouses = validationContext.GetService(typeof(WarehouseRepository)) as WarehouseRepository;

      if (warehouses.HasNumber(auth.User.id, (string) value)) {
        return new ValidationResult("Warehouse number has been used");
      }

      return ValidationResult.Success;
    }
  }
}
