using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Renet.Web;
using DBCore.Postgres;

namespace Tiantong.Wms.Api
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddSingleton<Config>();
      services.AddSingleton<Mail>();
      services.AddHostedService<MailHostedService>();

      services.AddControllers();
      services.AddHttpContextAccessor();
      services.AddSingleton<PostgresBuilder, DbBuilder>();
      services.AddDbContext<DbContext>();
      services.AddScoped<MigratorProvider>();
      services.AddSingleton<IHash, Hash>();
      services.AddSingleton<IRandom, Random>();

      services.AddScoped<Auth>();
      services.AddScoped<UserRepository>();
      services.AddScoped<PasswordResetRepository>();
      services.AddScoped<EmailVerificationRepository>();
      services.AddScoped<WarehouseRepository>();
      services.AddScoped<DepartmentRepository>();
      services.AddScoped<AreaRepository>();
      services.AddScoped<LocationRepository>();
      services.AddScoped<ProjectRepository>();
      services.AddScoped<SupplierRepository>();
      services.AddScoped<GoodCategoryRepository>();
      services.AddScoped<GoodRepository>();
      services.AddScoped<ItemRepository>();
      services.AddScoped<StockRepository>();
      services.AddScoped<OrderRepository>();
      services.AddScoped<BaseOrderRepository>();
      services.AddScoped<RequisitionOrderRepository>();
      services.AddScoped<PurchaseRequisitionOrderRepository>();
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseProvider<ExceptionHandler>();
      app.UseMiddleware<JsonBody>();
      app.UseRouting();
      app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
      app.UseProvider<WebRoutes>();
    }
  }
}
