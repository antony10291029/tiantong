using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Renet.Web.Kestrel
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
        .ConfigureWebHostDefaults(webBuilder => {
          webBuilder.UseStartup<Startup>()
            .UseUrls("http://localhost:5000");
        });
    }
  }

  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      var fileProvider = new ManifestEmbeddedFileProvider(typeof(Program).Assembly, "dist");

      app.UseFileServer(new FileServerOptions {
        RequestPath = "",
        FileProvider = fileProvider,
        EnableDirectoryBrowsing = true,
      });
    }
  }

}
