using Midos.Domain;
using System.Text.Json;

namespace Midos.Services.Http
{
  public record Data: object
  {

  }

  public record HttpPost: DomainEvent
  {
    public const string @event = "midos.http.post";

    public const string Event = "midos.http.post";

    public string Url { get; init; }

    public Data Data { get; init; }

    public static HttpPost From(string url, object data)
      => new() {
        Url = url,
        Data = data as Data
      };
  }
}
