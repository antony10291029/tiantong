using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using DBCore;

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
    private Dictionary<int, PlcWorker> _plcById = new Dictionary<int, PlcWorker>();

    private Dictionary<string, PlcWorker> _plcByName = new Dictionary<string, PlcWorker>();

    public PlcManager(IServiceProvider services, IServiceScopeFactory scopeFactory)
    {
      var domain = services.GetService<DomainContextFactory>();

      domain.Migrate();

      using (var scope = scopeFactory.CreateScope()) {
        var systemRepository = scope.ServiceProvider.GetService<SystemRepository>();
        var builder = scope.ServiceProvider.GetService<PlcBuilder>();

        var plcRepository = scope.ServiceProvider.GetService<PlcRepository>();
        var isAutorun = systemRepository.GetIsAutorun();

        if (isAutorun) {
          var plcs = plcRepository.AllWithRelationships();
          var workers = plcs.Select(plc => builder.BuildWorker(plc)).ToArray();

          foreach (var worker in workers) {
            Run(worker);
          }
        }
      }
    }

    public PlcWorker Get(int id)
    {
      return _plcById[id];
    }

    public PlcWorker Get(string name)
    {
      return _plcByName[name];
    }

    public bool Run(PlcWorker worker)
    {
      var id = worker.Client().Options().Id();
      var name = worker.Client().Options().Name();

      if (_plcById.ContainsKey(id)) {
        return false;
      } else {
        _plcById[id] = _plcByName[name] = worker;
        Task.Run(worker.RunAsync);

        return true;
      }
    }

    public bool Stop(int id)
    {
      if (_plcById.ContainsKey(id)) {
        var worker = _plcById[id];
        var name = worker.Client().Options().Name();

        try {
          worker.Stop();
        } finally {
          _plcById.Remove(id);
          _plcByName.Remove(name);
        }

        return true;
      } else {
        return false;
      }
    }

    public PlcManager Stop()
    {
      foreach (var plc in _plcById.Values) {
        Stop(plc.Client().Options().Id());
      }

      return this;
    }

    public async Task WaitAsync()
    {
      await Task.WhenAll(_plcById.Values.Select(plc => plc.WaitAsync()));
    }

  }

}
