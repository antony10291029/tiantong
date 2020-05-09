using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;

namespace Tiantong.Iot.Api
{
  public class PlcManagerService : BackgroundService
  {
    private PlcManager _plcManager;

    public PlcManagerService(PlcManager plcManager)
    {
      _plcManager = plcManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      
      while (!stoppingToken.IsCancellationRequested) {
        await Task.Delay(100);
      }

      await _plcManager.Stop().WaitAsync();
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
      await base.StopAsync(stoppingToken);
    }
  }

  public class PlcManager
  {
    private Dictionary<int, IPlcWorker> _plcs = new Dictionary<int, IPlcWorker>();

    public void Start()
    {
      foreach (var plc in _plcs.Values) {
        plc.Start();
      }
    }

    public void Add(PlcWorker worker)
    {
      var id = worker._id;
      if (_plcs.ContainsKey(id)) {
        Stop(id);
      }

      _plcs[id] = worker;
      worker.Start();
    }

    public void Stop(int id)
    {
      _plcs[id].Stop();
    }

    public PlcManager Stop()
    {
      foreach (var plc in _plcs.Values) {
        plc.Stop();
      }

      return this;
    }

    public async Task WaitAsync()
    {
      await Task.WhenAll(_plcs.Values.Select(plc => plc.WaitAsync()));
    }
  }

}
