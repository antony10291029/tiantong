using System;
using System.ComponentModel.DataAnnotations;

namespace Tiantong.Wms.Api
{
  public class WarehouseOwnerCheckAttribute : ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      var id = (int) value;
      var auth = validationContext.GetService(typeof(IAuth)) as IAuth;
      var warehouses = validationContext.GetService(typeof(WarehouseRepository)) as WarehouseRepository;

      if (!warehouses.HasId(id)) {
        return new ValidationResult("Warehouse id does not exist");
      }
      if (!warehouses.HasOwner(id, auth.User.id)) {
        return new ValidationResult("Warehouse owner check failed");
      }

      return ValidationResult.Success;
    }
  }
}
