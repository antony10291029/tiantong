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
      Post("/dev/migrate", "App.Migrate");
      Post("/dev/rollback", "App.Rollback");
      Post("/dev/refresh", "App.Refresh");
      Post("/dev/reseed", "App.Reseed");

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
      Post("/warehouses/find", "Warehouse.Find");

      Post("/warehouses/users/create", "WarehouseUser.Create");
      Post("/warehouses/users/delete", "WarehouseUser.Delete");
      Post("/warehouses/users/update", "WarehouseUser.Update");
      Post("/warehouses/users/find", "WarehouseUser.Find");
      Post("/warehouses/users/all", "WarehouseUser.All");

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
      Post("/projects/find", "Project.Find");
      Post("/projects/all", "Project.All");

      Post("/departments/create", "Department.Create");
      Post("/departments/delete", "Department.Delete");
      Post("/departments/update", "Department.Update");
      Post("/departments/find", "Department.Find");
      Post("/departments/all", "Department.All");

      Post("/good-categories/create", "GoodCategory.Create");
      Post("/good-categories/delete", "GoodCategory.Delete");
      Post("/good-categories/update", "GoodCategory.Update");
      Post("/good-categories/search", "GoodCategory.Search");
      Post("/good-categories/find", "GoodCategory.Find");

      Post("/orders/categories/create", "OrderCategory.Create");
      Post("/orders/categories/delete", "OrderCategory.Delete");
      Post("/orders/categories/update", "OrderCategory.Update");
      Post("/orders/categories/search", "OrderCategory.Search");

      Post("/goods/create", "Good.Create");
      Post("/goods/delete", "Good.Delete");
      Post("/goods/update", "Good.Update");
      Post("/goods/search", "Good.Search");
      Post("/goods/find", "Good.Find");

      Post("/goods/items/create", "Good.AddGoodItem");

      Post("/items/create", "Item.Create");
      Post("/items/delete", "Item.Delete");

      Post("/suppliers/create", "Supplier.Create");
      Post("/suppliers/delete", "Supplier.Delete");
      Post("/suppliers/update", "Supplier.Update");
      Post("/suppliers/search", "Supplier.Search");
      Post("/suppliers/find", "Supplier.Find");
      Post("/suppliers/all", "Supplier.All");

      Post("/purchase-orders/create", "PurchaseOrder.Create");
      Post("/purchase-orders/update", "PurchaseOrder.Update");
      Post("/purchase-orders/search", "PurchaseOrder.Search");
      Post("/purchase-orders/find", "PurchaseOrder.Find");

    }
  }
}
