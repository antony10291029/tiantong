using System.Threading.Tasks;
using System.Text;
namespace Tiantong.Iot.Test
{
  public class TestWatcherHttpClient: IWatcherHttpClient
  {
    public string Url;

    public string Data;

    public Task PostAsync(int id, string url, string data, Encoding encoding = null)
    {
      Url = url;
      Data = data;
      return Task.CompletedTask;
    }
  }
}
