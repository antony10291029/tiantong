using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Namei.ApiGateway.Server
{
  public class HttpLogService
  {
    private readonly object _lock = new();

    private bool _isSaving = false;

    private readonly ConcurrentBag<HttpLog> _logs = new();

    private readonly IServiceProvider _serviceProvider;

    public HttpLogService(IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider;
    }

    public void Add(HttpLog log)
    {
      if (_isSaving) {
        lock(_lock) {
          _logs.Add(log);
        }
      } else {
        _logs.Add(log);
      }
    }

    public async Task SaveAsync()
    {
      HttpLog[] logs;

      _isSaving = true;

      lock(_lock) {
        logs = _logs.ToArray();
        _logs.Clear();
      }

      _isSaving = false;

      if (logs.Length != 0) {
        using var scope = _serviceProvider.CreateScope();
        using var context = scope.ServiceProvider.GetService<AppContext>();

        context.AddRange(logs);
        await context.SaveChangesAsync();
      }
    }
  }
}
