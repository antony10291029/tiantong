using Microsoft.AspNetCore.Mvc;
using Midos.SeedWork.Domain;

namespace Namei.ApiGateway.Server
{ 
  [Controller]
  [Route("$endpoints")]
  public class EndpointController: EntityController<Endpoint>
  {
    public EndpointController(
      IRepository<Endpoint> repository
    ): base(repository) {

    }
  }
}
