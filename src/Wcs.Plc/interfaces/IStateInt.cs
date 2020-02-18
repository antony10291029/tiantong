using System.Threading.Tasks;

namespace Wcs.Plc
{
  public interface IStateInt : IState<int>
  {
    IStateInt Heartbeat(int times = 100, int maxTimes = 10000);

    Task UnheartbeatAsync();

    void Unheartbeat();
  }
}
