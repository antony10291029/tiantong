using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Midos.Domain;
using System.Linq;

namespace Namei.ApiGateway.Server
{ 
  [Controller]
  [Route(Name)]
  public class RouteController: EntityController<Route>
  {
    const string Name = "$routes";

    public RouteController(
      DatabaseContext context,
      ILogger<Route> logger
    ): base(context, logger) {

    }

    protected override IQueryable<Route> AsQueryable(DbSet<Route> set)
      => set.Include(route => route.Endpoint);

    protected override void HandleReference(Route entity)
    {
      entity.Endpoint.Routes = null;
    }
  }
}
