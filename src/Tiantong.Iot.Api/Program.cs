using System.Reflection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Tiantong.Iot.Api
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
          config.UsePort(port);
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

    public static IConfigurationBuilder UsePort(this IConfigurationBuilder config, string port)
    {
      if (port != null) {
        config.AddInMemoryCollection(
          new Dictionary<string, string>() {
            { "urls", $"http://*:{port}" }
          }
        );
      }

      return config;
    }
  }
}
