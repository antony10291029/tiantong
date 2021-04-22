using Midos.Domain;
using System.Text.Json;

namespace Namei.Wcs.Aggregates
{
  public record WebHookPost: DomainEvent
  {
    public const string Message = "webhook.post";

    public string Url { get; init; }

    public string Data { get; init; }

    public WebHookPost() {}

    public WebHookPost(string url, string data)
    {
      Url = url;
      Data = data;
    }

    public WebHookPost(string url, object data)
    {
      Url = url;
      Data = JsonSerializer.Serialize(data);
    }
  }
}
