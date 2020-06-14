using System;
using AspNetCore.Proxy;
using AspNetCore.Proxy.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Renet.Web;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Yuchuan.IErp.Proxy.Chanjet
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args = null)
    {
      return Host.CreateDefaultBuilder(args)
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

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration config)
    {
      app.UseProvider<ExceptionHandler>();
      app.UseCors(cors =>
        cors.AllowAnyHeader().AllowAnyMethod().AllowCredentials()
          .WithOrigins(config.GetValue("client_url", "http://localhost:8080"))
      );
      app.RunProxy(proxy => proxy.UseHttp(
        (context, args) => {
          if (
            context.Request.Path.StartsWithSegments("/hc") ||
            context.Request.Path.StartsWithSegments("/logout")
          ) {
            return "http://localhost:5001";
          } else if (
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
            .WithCookieProxy(env);
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

    public static IHttpProxyOptionsBuilder WithCookieProxy(this IHttpProxyOptionsBuilder proxy, IWebHostEnvironment env)
    {
      return proxy.WithAfterReceive(async (context, msg) => {
        var options = new CookieOptions {
          Domain = env.IsProduction() ? ".als-yuchuan.com" : "localhost"
        };

        if (msg.Headers.Contains("Set-Cookie")) {
          foreach (var cookie in msg.Headers.GetValues("Set-Cookie")) {
            var keyValue = cookie.Split("; ")[0].Split("=");

            context.Response.Cookies.Append(keyValue[0], keyValue[1], options);
          }

          msg.Headers.Remove("Set-Cookie");
        }

        if (context.Request.Path.Value == "/logout") {
          context.Response.Cookies.Append("CIC", "", options);
        }

        await Task.CompletedTask;
      });
    }
  }
}