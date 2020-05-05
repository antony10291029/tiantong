using System.Text;
using System.Threading.Tasks;

namespace Tiantong.Iot
{
  public interface IWatcherHttpClient
  {
    Task PostAsync(int plcId, int stateId, int watcherId, string url, string data, Encoding encoding = null);
  }
}
