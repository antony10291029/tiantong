using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Midos.SeedWork.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Namei.ApiGateway.TestServer
{
  public class TestHostedService: IntervalService
  {
    private readonly IServiceProvider _serviceProvider;

    private readonly Midos.SeedWork.Services.Random _random;

    private readonly HttpClient _client;

    private Route[] _routes;

    public TestHostedService(
      IServiceProvider serviceProvider,
      Midos.SeedWork.Services.Random random,
      IHttpClientFactory factory
    ) {
      Time = 1;
      _serviceProvider = serviceProvider;
      _random = random;
      _client = factory.CreateClient();
      Initialize();
    }

    private void Initialize()
    {
      using var scope = _serviceProvider.CreateScope();
      using var context = scope.ServiceProvider.GetService<AppContext>();

      _routes = context.Set<Route>()
        .Include(route => route.Endpoint)
        .AsNoTracking()
        .ToArray();
    }

    protected override async Task HandleJob(CancellationToken stoppingToken)
    {
      var route = _random.Array(_routes);
      var path = "http://172.16.2.64:5200" + route.Path;

      var data = new Dictionary<string, object>();

      for (int i = 0, L = _random.Int(1, 10); i < L; i++) {
        data[$"test_property_key_{i}"] = $"test_property_value_{i}";
      }


      await _client.PostAsJsonAsync(path, data as object, null, stoppingToken);
    }
  }
}
