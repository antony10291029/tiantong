using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class WebRoutes : RouterProvider
  {
    public override void Define()
    {
      Get("/", "App.Home");
      Post("/", "App.Post");
      Get("/users", "App.Users");
      Get("/error", "App.Error");
      Get("/error/unexpected", "App.UnexpectedError");

      Post("/validate", "App.Validate");
      Post("/validate/customer", "App.CustomerValidate");
    }
  }
}
