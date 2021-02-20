using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Midos.Extensions;

namespace Namei.Wcs.Api
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args, string port = null)
    {
      return Host.CreateDefaultBuilder(args)
        .ConfigureEnvironment()
        .ConfigureWebHostDefaults(builder => {
          builder.UseIISIntegration();
          builder.UseStartup<Startup>();
        });
    }
  }
}
