using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Tiantong.Wms.DB;

namespace Tiantong.Wms.Api
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var migrator = new PostgresMigrator();
      migrator.Migrate();
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
      return Host.CreateDefaultBuilder(args)
        .UseEnvironment("Development")
        .ConfigureWebHostDefaults(builder => {
          builder.UseStartup<Startup>();
        });
    }
  }
}
