using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Midos.Domain;

namespace Namei.ApiGateway.Server
{
  [Index(nameof(Path), IsUnique = true)]
  [Index(nameof(EndpointPath), nameof(EndpointId), IsUnique = true)]
  public class Route: IEntity
  {
    [Key]
    public long Id { get; set; }

    public string Path { get; set; }

    public string EndpointPath { get; set; }

    public string Name { get; set; }

    public long EndpointId { get; set; }

    public Endpoint Endpoint { get; }

    public Route() {}

    public static Route From(
      string path,
      string name,
      string endpointPath,
      long endpointId
    ) => new() {
      Path = path,
      Name = name,
      EndpointPath = endpointPath,
      EndpointId = endpointId
    };
  }
}
