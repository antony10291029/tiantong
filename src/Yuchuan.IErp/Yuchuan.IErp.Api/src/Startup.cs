using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Renet.Utils;
using Renet.Web;
using Tiantong.Account.Utils;

namespace Yuchuan.IErp.Api
{
  public class Startup
  {
    private Config _config;

    public Startup(IConfiguration conf, IHostEnvironment env)
    {
      _config = new Config(conf, env);
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddHttpContextAccessor();
      services.AddControllers();
      services.AddSingleton<Config>();
      services.AddSingleton<IHash, Hash>();
      services.AddSingleton<IRandom, Random>();
      services.AddSingleton<DbBuilder>();
      services.AddScoped<Auth>();
      services.AddScoped<DomainContext>();
      services.AddScoped<MigratorProvider>();
      services.AddScoped<DeviceStateRepository>();
      services.AddHttpClient<TokenService>();
      services.AddSignalR().AddStackExchangeRedis(_config.SIGNALR_REDIS);
      services.AddStackExchangeRedisCache(options =>
        options.Configuration = _config.CACHE_REDIS
      );
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseMiddleware<JsonBody>();
      app.UseProvider<ExceptionHandler>();
      app.UseRouting();
      app.UseCors(Cors);
      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
        endpoints.MapHub<DeviceStateHub>("/device-states");
      });
    }

    private System.Action<CorsPolicyBuilder> Cors => policy => policy
      .WithOrigins(_config.CORS_ORIGIN)
      .AllowAnyHeader()
      .AllowAnyMethod()
      .AllowCredentials();
  }
}
