using System.Text;
using System.Threading.Tasks;

namespace Tiantong.Iot
{
  public interface IWatcherHttpClient
  {
    Task PostAsync(int id, string url, string data, Encoding encoding = null);
  }
}
