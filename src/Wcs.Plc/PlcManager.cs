using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Wcs.Plc
{
  public class PlcManager
  {
    public Dictionary<string, IPlc> Plcs = new Dictionary<string, IPlc>();

    public void Start()
    {
      foreach (var plc in Plcs.Values) {
        plc.Start();
      }
    }

    public PlcManager Stop()
    {
      foreach (var plc in Plcs.Values) {
        plc.Stop();
      }

      return this;
    }

    public async Task WaitAsync()
    {
      await Task.WhenAll(Plcs.Values.Select(plc => plc.WaitAsync()));
    }
  }
}
