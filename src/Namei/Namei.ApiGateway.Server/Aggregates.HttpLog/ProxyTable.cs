using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Namei.ApiGateway.Server
{
  public class ProxyRecord
  {
    public long EndpointId { get; set; }

    public long RouteId { get; set; }

    public string Path { get; set; }

    public string Endpoint { get; set; }

    public string EndpointPath { get; set; }

    public string EndpointFullPath { get => Endpoint + EndpointPath; }
  }

  public class ProxyTable
  {
    private IReadOnlyDictionary<string, ProxyRecord> _table;

    private readonly IServiceProvider _serviceProvider;

    public ProxyTable(IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider;
      Sync();
    }

    public IReadOnlyDictionary<string, ProxyRecord> Get()
      => _table.ToImmutableDictionary();

    public void Sync()
    {
      _table = Fetch();
    }

    public IReadOnlyDictionary<string, ProxyRecord> Fetch()
    {
      using var scope = _serviceProvider.CreateScope();
      using var context = scope.ServiceProvider.GetService<AppContext>();

      return context.Set<Endpoint>()
        .Include(endpoint => endpoint.Routes)
        .ToArray()
        .SelectMany(endpoint => endpoint.Routes
          .Select(route => new ProxyRecord {
            EndpointId = endpoint.Id,
            RouteId = route.Id,
            Path = route.Path,
            Endpoint = endpoint.Url,
            EndpointPath = route.EndpointPath,
          })
        )
        .OrderBy(record => record.Path)
        .ToDictionary(record => record.Path, record => record);
    }
  }
}
