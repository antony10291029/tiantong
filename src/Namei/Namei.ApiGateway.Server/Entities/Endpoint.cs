using System.Collections.Generic;
using Midos.Domain;

namespace Namei.ApiGateway.Server
{
  public class Endpoint: IEntity
  {
    public long Id { get; set; }

    public string Url { get; set; }

    public string Name { get; set; }

    public List<Route> Routes { get; }

    public Endpoint() {}

    public static Endpoint From(
      string url,
      string name
    ) => new() {
      Url = url,
      Name = name
    };
  }
}
