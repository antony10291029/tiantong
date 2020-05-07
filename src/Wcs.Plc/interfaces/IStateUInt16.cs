using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Wcs.Plc
{
  public interface IStateUInt16 : IState<ushort>
  {
    IState<ushort> Heartbeat(int interval = 100, ushort maxTimes = 10000);

    Task UnheartbeatAsync();

    void Unheartbeat();
  }
}
