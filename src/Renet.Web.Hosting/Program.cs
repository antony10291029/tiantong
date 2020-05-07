using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Renet.Web.Example
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder().Build().Run();
    }

    public static IHostBuilder CreateHostBuilder() =>
      Host.CreateDefaultBuilder()
        .ConfigureServices((hostContext, services) =>
        {
          services.AddHostedService<Worker>();
        });
  }
}
