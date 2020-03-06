using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class WebRoutes : RouterProvider
  {
    public override void Define()
    {
      Get("/", "App.Home");
      Post("/", "App.Home");

      Post("/dev/initialize", "App.Initialize");
      Post("/dev/restore", "App.Restore");
      Post("/dev/seed", "App.InsertTestData");

      Post("/users/search", "User.Search");

      Post("/users/register", "User.Register");
      Post("/users/person/update", "User.UpdatePerson");
      Post("/users/person/profile", "User.GetPersonProfile");

      Post("/auth/email", "User.AuthByEmail");
      Post("/auth/token/refresh", "User.RefreshToken");

      Post("/warehouses/create", "Warehouse.Create");
      Post("/warehouses/delete", "Warehouse.Delete");
      Post("/warehouses/update", "Warehouse.Update");
      Post("/warehouses/search", "Warehouse.Search");

      Post("/areas/create", "Area.Create");
      Post("/areas/delete", "Area.Delete");
      Post("/areas/update", "Area.Update");
      Post("/areas/search", "Area.Search");

      Post("/locations/create", "Location.Create");
      Post("/locations/delete", "Location.Delete");
      Post("/locations/update", "Location.Update");
      Post("/locations/search", "Location.Search");

      Post("/projects/create", "Project.Create");
      Post("/projects/delete", "Project.Delete");
      Post("/projects/update", "Project.Update");
      Post("/projects/search", "Project.Search");

      Post("/items/categories/create", "ItemCategory.Create");
      Post("/items/categories/delete", "ItemCategory.Delete");
      Post("/items/categories/update", "ItemCategory.Update");
      Post("/items/categories/search", "ItemCategory.Search");

      Post("/orders/categories/create", "OrderCategory.Create");
      Post("/orders/categories/delete", "OrderCategory.Delete");
      Post("/orders/categories/update", "OrderCategory.Update");
      Post("/orders/categories/search", "OrderCategory.Search");

      Post("/items/create", "Item.Create");
      Post("/items/delete", "Item.Delete");
      Post("/items/update", "Item.Update");
      Post("/items/search", "Item.Search");

      Post("/stocks/search", "Stock.Search");

    }
  }
}
