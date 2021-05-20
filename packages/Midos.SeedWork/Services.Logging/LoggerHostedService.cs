using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Midos.SeedWork.Services.Logging
{
  public class LoggerHostedService: IntervalService
  {
    private readonly IServiceProvider _serviceProvider;

    private readonly string _appName;

    private readonly string _server;

    public LoggerHostedService(IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider;
      _appName = serviceProvider.GetService<IAppInfo>()?.AppName ?? "None";
      _server = serviceProvider.GetService<IAppInfo>()?.Server ?? "None";
    }

    public override Task StartAsync(CancellationToken stoppingToken)
    {
      Time = 1000;
      using var scope = _serviceProvider.CreateScope();
      using var context = scope.ServiceProvider.GetService<LoggerContext>();

      context.Database.EnsureCreated();

      return base.StartAsync(stoppingToken);
    }

    protected override Task HandleJob(CancellationToken stoppingToken)
    {
      var provider =  _serviceProvider.GetService<MidosLoggerProvider>();
      var logs = provider.Loggers().SelectMany(logger => logger.Clear()).ToArray();

      foreach (var log in logs) {
        log.App = _appName;
        log.Server = _server;
      }

      if (logs.Length != 0) {
        using var scope = _serviceProvider.CreateScope();
        using var context = scope.ServiceProvider.GetService<LoggerContext>();

        context.AddRange(logs);
        context.SaveChanges();
      }

      return Task.CompletedTask;
    }
  }
}
