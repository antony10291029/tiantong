namespace Tiantong.Iot.Test
{
  public class TestWatcherProvider: WatcherProvider
  {
    static IWatcherHttpClient _httpClient = new TestWatcherHttpClient();

    public TestWatcherProvider(): base(_httpClient)
    {

    }
  }
}
