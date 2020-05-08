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

    public IPlcWorker Get(int id)
    {
      return _plcs[id];
    }

    public bool Run(PlcWorker worker)
    {
      if (_plcs.ContainsKey(worker._id)) {
        return false;
      } else {
        _plcs[worker._id] = worker;
        worker.Start();

        return true;
      }
    }

    public bool Stop(int id)
    {
      if (_plcs.ContainsKey(id)) {
        _plcs[id].Stop().WaitAsync()
          .ContinueWith(task => {
            task.GetAwaiter().GetResult();
            _plcs.Remove(id);
          });

        return true;
      } else {
        return false;
      }
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
