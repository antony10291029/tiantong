using Midos.Domain;

namespace Midos.Services.Http
{
  public record HttpLogEvent: DomainEvent
  {
    public const string @event = "midos.http.before.send";

    public string Method { get; init; }

    public string Url { get; init; }

    public int Status { get; init; }

    public string Request { get; init; }

    public string Response { get; init; }

    public static HttpLogEvent From(
      string method,
      string url,
      int status,
      string request,
      string response
    ) => new() {
      Method = method,
      Url = url,
      Status = status,
      Request = request,
      Response = response
    };
  }
}
