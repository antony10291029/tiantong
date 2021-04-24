using Midos.Domain;
using System.Text.Json;

namespace Midos.Services.Http
{
  public record HttpPost: DomainEvent
  {
    public const string Event = "midos.http.post";

    public string Url { get; init; }

    public string Data { get; init; }

    public static HttpPost From(string url, string data)
      => new HttpPost {
        Url = url,
        Data = data
      };

    public static HttpPost From(string url, object data)
      => From(
        url: url,
        data: JsonSerializer.Serialize(data)
      );
  }
}
