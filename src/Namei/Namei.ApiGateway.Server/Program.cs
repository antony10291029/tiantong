using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Namei.ApiGateway.Server
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
      return Host.CreateDefaultBuilder(args)
        .ConfigureEnvironment()
        // .UseMidosLogging()
        .ConfigureWebHostDefaults(builder => {
          builder.UseIISIntegration();
          builder.UseStartup<Startup>();
        });
    }
  }
}
