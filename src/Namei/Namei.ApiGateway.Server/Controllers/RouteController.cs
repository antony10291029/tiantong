using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Midos.SeedWork.Domain;

namespace Namei.ApiGateway.Server
{ 
  public class RouteRepository: Repository<Route>
  {
    private readonly AppContext _context;

    public RouteRepository(AppContext context, IUnitOfWork unitOfWork)
      : base(context, unitOfWork)
    {
      _context = context;
    }

    public override DataMap<Route> Query(QueryParams param)
    {
      return _context.Set<Route>()
        .Include(route => route.Endpoint)
        .OrderByDescending(route => route.Endpoint.Id)
          .ThenByDescending(route => route.Id)
        .ToDataMap();
    }
  }

  [Controller]
  [Route("$routes")]
  public class RouteController: EntityController<Route>
  {
    public RouteController(
      RouteRepository repository
    ): base(repository) {

    }
  }
}
