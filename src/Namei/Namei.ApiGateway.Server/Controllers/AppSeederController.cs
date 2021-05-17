using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Midos.SeedWork.Domain;
using System.Linq;

namespace Namei.ApiGateway.Server
{
  public class AppSeederController: SeederController
  {
    private readonly AppContext _context;

    public AppSeederController(
      AppContext context,
      IHostEnvironment env,
      ILogger<SeederController> logger
    ): base(context, env, logger) {
      _context = context;
    }

    protected override void Seed()
    {
      InsertEndpoints();
      InsertRoutes();
    }

    private void InsertEndpoints()
    {
      _context.Add(
        Endpoint.From(
          url: "http://localhost:5000",
          name: "测试应用"
        )
      );
      _context.AddRange(
        Enumerable.Range(1, 20)
          .Select(i => Endpoint.From(
            url: $"http://localhost:500{i}",
            name: $"测试应用_{i}"
          ))
      );

      _context.SaveChanges();
    }

    private void InsertRoutes()
    {
      var count = 0;
      var endpionts = _context.Set<Endpoint>().ToArray();
      var routes = endpionts.SelectMany(endpoint =>
        Enumerable.Range(1, 20)
          .Select(i => Route.From(
            path: $"/test/{++count}",
            name: $"测试接口_{count}",
            endpointPath: $"/test/endpoint/{count}",
            endpointId: endpoint.Id
          )
        )
      );

      _context.AddRange(routes);
      _context.SaveChanges();
    }
  }
}
