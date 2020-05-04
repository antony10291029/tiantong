namespace Tiantong.Iot
{
  public class BaseWatcherProvider: IWatcherProvider
  {
    public IWatcherHttpClient HttpClient { get;  } = new TestHttpClient();

    public IWatcher<T> Resolve<T>()
    {
      return new Watcher<T>(HttpClient);
    }
  }

  public class WatcherHttpClient: IWatcherHttpClient
  {
    public void Post(string url, string data)
    {

    }
  }

  public class TestHttpClient: IWatcherHttpClient
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
