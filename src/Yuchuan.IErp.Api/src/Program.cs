using System.Reflection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Yuchuan.IErp.Api
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
        .UseEnvironment("Development")
        .ConfigureAppConfiguration((context, config) => {
          config.UseSettings();
        })
        .ConfigureWebHostDefaults(builder => {
          builder.UseStartup<Startup>();
        });
    }
  }

  public static class ProgramExtensions
  {
    public static IConfigurationBuilder UseSettings(this IConfigurationBuilder builder)
    {
      builder.AddJsonFile(
        new EmbeddedFileProvider(Assembly.GetExecutingAssembly()),
        "settings.json", false, false
      );

      return builder;
    }
  }
}
