namespace Tiantong.Iot.Api
{
  public class HttpPusherFactory
  {
    private HttpPusherClient _client;

    public HttpPusherFactory(HttpPusherClient client)
    {
      _client = client;
    }

    public HttpPusher<T> Resolve<T>()
    {
      return new HttpPusher<T>(_client);
    }

  }

}
