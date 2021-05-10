using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Midos.Domain;

namespace Namei.ApiGateway.Server
{ 
  [Controller]
  [Route(Name)]
  public class EndpointController: EntityController<Endpoint>
  {
    const string Name = "$endpoints";

    public EndpointController(
      DatabaseContext context,
      ILogger<Endpoint> logger
    ): base(context, logger) {

    }
  }
}
