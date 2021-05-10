using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Midos.Domain;

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
  }
}
