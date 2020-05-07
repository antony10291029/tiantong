using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Wcs.Plc
{
  public interface IStateInt32 : IState<int>
  {
    IState<int> Heartbeat(int times = 100, int maxTimes = 10000);

    Task UnheartbeatAsync();

    void Unheartbeat();
  }
}
