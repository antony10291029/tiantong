using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Namei.Common.Api
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
        .ConfigureWebHostDefaults(builder => {
          builder.UseIISIntegration();
          builder.UseStartup<Startup>();
        });
    }
  }
}