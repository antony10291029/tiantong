using System.Threading.Tasks;

namespace Wcs.Plc
{
  public interface IStateWord : IState<int>
  {
    IStateWord Heartbeat(int times = 1000, int maxTimes = 10000);

    Task UnheartbeatAsync();

    void Unheartbeat();
  }
}
