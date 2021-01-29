using Microsoft.AspNetCore.Mvc.Filters;

namespace Renet.Web
{
  public class ValidateModelAttribute : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext context)
    {
      if (!context.ModelState.IsValid) {
        throw new HttpModelValidationException(context.ModelState);
      }
    }
  }
}
