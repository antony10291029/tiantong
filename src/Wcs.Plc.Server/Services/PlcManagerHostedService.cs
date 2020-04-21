using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Wcs.Plc.Server
{
  public class PlcManagerHostedService : BackgroundService
  {
    public PlcManager PlcManager;

    public PlcManagerHostedService(PlcManager manager)
    {
      PlcManager = manager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      PlcManager.Start();
      Console.WriteLine("-----Plc Manager Started");

      while (!stoppingToken.IsCancellationRequested) {
        await Task.Delay(100);
      }

      await PlcManager.Stop().WaitAsync();
      Console.WriteLine("-----Plc Manager Stopped");
    }

  }
}
