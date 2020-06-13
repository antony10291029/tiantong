using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using AspNetCore.Proxy;
using AspNetCore.Proxy.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace Yuchuan.IErp.Proxy.Chanjet
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args = default(string[]))
    {
      return Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, config) => {
          config.UseEmbeddedFile("settings.json");
        })
        .ConfigureWebHostDefaults(webBuilder => {
          webBuilder.UseStartup<Startup>();
        });
    }
  }

  public class Startup
  {
    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddProxies();
      services.AddCors();
      services.AddHttpClient("master")
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler() {
          UseCookies = false,
          UseDefaultCredentials = false
        });
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:8080"));
      app.RunProxy(proxy => proxy.UseHttp(
        (context, args) => {
          if (
            context.Request.Path.StartsWithSegments("/vm") ||
            context.Request.Path.StartsWithSegments("/loginV2")
          ) {
            return "https://passport.chanjet.com";
          } else if (context.Request.Path.StartsWithSegments("/internal_api")) {
            return "https://cia.chanapp.chanjet.com";
          } else {
            return "https://cloud.chanjet.com";
          }
        },
        builder => {
          builder.WithHttpClientName("master")
            .WithCookieProxy();
        }
      ));
    }
  }

  public static class ProgramExtensions
  {
    public static IConfigurationBuilder UseEmbeddedFile(this IConfigurationBuilder builder, params string[] files)
    {
      foreach (var file in files) {
        builder.AddJsonFile(
          new EmbeddedFileProvider(Assembly.GetExecutingAssembly()),
          file, false, false
        );
      }

      return builder;
    }

    public static IHttpProxyOptionsBuilder WithCookieProxy(this IHttpProxyOptionsBuilder proxy)
    {
      return proxy.WithAfterReceive(async (context, msg) => {
        if (msg.Headers.Contains("Set-Cookie")) {
          foreach (var cookie in msg.Headers.GetValues("Set-Cookie")) {
            var keyValue = cookie.Split("; ")[0].Split("=");

            Console.WriteLine(keyValue[0]);
            Console.WriteLine(keyValue[1]);
            context.Response.Cookies.Delete(keyValue[0]);
            context.Response.Cookies.Append(keyValue[0], keyValue[1]);
          }

          msg.Headers.Remove("Set-Cookie");
        }

        await Task.CompletedTask;
      });
    }
  }
}