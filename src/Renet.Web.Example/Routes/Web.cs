namespace Renet.Web.Example
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
    }
  }
}
