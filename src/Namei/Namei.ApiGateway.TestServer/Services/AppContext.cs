using Microsoft.EntityFrameworkCore;
using Midos.SeedWork.Domain;

namespace Namei.ApiGateway.TestServer
{
  public class AppContext: EFContext
  {
    protected DbSet<Endpoint> Endpoints { get; set; }

    protected DbSet<Route> Routes { get; set; }

    public AppContext(AppContextOptions options): base(options)
    {

    }
  }
}
