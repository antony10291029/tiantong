using System.ComponentModel.DataAnnotations;

namespace Tiantong.Wms.Api
{
  public class UserEmailUniqueAttribute : ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      var users = validationContext.GetService(typeof(UserRepository)) as UserRepository;

      if (users.HasEmail((string) value)) {
        return new ValidationResult("Email has been used");
      }

      return ValidationResult.Success;
    }
  }
}
