namespace Tiantong.Iot
{
  public class WatcherProvider: IWatcherProvider
  {
    public IWatcherHttpClient HttpClient { get; }

    public WatcherProvider(IWatcherHttpClient httpClient)
    {
      HttpClient = httpClient;
    }

    public IWatcher<T> Resolve<T>()
    {
      return new Watcher<T>(HttpClient);
    }
  }

}
