namespace Tiantong.Iot
{
  public interface IWatcherProvider
  {
    IWatcherHttpClient HttpClient { get; }

    IWatcher<T> Resolve<T>();
  }

}
