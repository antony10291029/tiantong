namespace Tiantong.Iot.Test
{
  public class TestWatcherHttpClient: IWatcherHttpClient
  {
    public string Url;

    public string Data;

    public void Post(string url, string data)
    {
      Url = url;
      Data = data;
    }
  }
}
