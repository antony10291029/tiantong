using Midos.Domain;

namespace Namei.Wcs.Aggregates
{
  public record WebHookPost: DomainEvent
  {
    public const string Message = "webhook.post";

    public string Url { get; init; }

    public string Data { get; init; }

    public WebHookPost(string url, string data)
    {
      Url = url;
      Data = data;
    }
  }
}
