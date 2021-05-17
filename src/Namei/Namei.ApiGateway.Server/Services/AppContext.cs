using Microsoft.EntityFrameworkCore;
using Midos.SeedWork.Domain;

namespace Namei.ApiGateway.Server
{
  public class AppContext: EFContext
  {
    protected DbSet<Endpoint> Endpoints { get; set; }

    protected DbSet<Route> Routes { get; set; }

    protected DbSet<HttpLog> HttpLogs { get; set; }

    public AppContext(AppContextOptions options): base(options)
    {

    }
  }
}
