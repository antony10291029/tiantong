using System;
namespace Tiantong.Iot
{
  public class WatcherHttpClient: IWatcherHttpClient
  {
    public void Post(string url, string data)
    {
      Console.WriteLine(url);
      Console.WriteLine(data);
    }
  }
}