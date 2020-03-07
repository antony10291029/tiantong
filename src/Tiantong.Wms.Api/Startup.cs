using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      services.AddHttpContextAccessor();
      services.AddDbContext<DbContext>();
      services.AddScoped<IAuth, Auth>();
      services.AddSingleton<IHash, Hash>();
      services.AddSingleton<IRandom, Random>();
      services.AddScoped<UserRepository>();
      services.AddScoped<WarehouseRepository>();
      services.AddScoped<KeeperRepository>();
      services.AddScoped<AreaRepository>();
      services.AddScoped<LocationRepository>();
      services.AddScoped<ProjectRepository>();
      services.AddScoped<SupplierRepository>();
      services.AddScoped<ItemCategoryRepository>();
      services.AddScoped<OrderCategoryRepository>();
      services.AddScoped<ItemRepository>();
      services.AddScoped<StockRepository>();
      services.AddScoped<OrderRepository>();
      services.AddScoped<OrderItemRepository>();
      services.AddScoped<StockRecordRepository>();
      services.AddScoped<OrderProjectRepository>();
      services.AddScoped<OrderSupplierRepository>();
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseProvider<ExceptionHandler>();
      app.UseMiddleware<JsonBody>();
      app.UseRouting();
      app.UseProvider<WebRoutes>();
    }
  }
}
