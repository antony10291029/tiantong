using System.Collections.Generic;

namespace Wcs.Plc.Test
{
  using Logs = List<EventArgs>;

  public class TestEventPlugin : EventPlugin
  {
    public Logs Logs = new Logs();

    public override void Install(Event event_)
    {
      event_.All(args => Logs.Add(args));
    }
  }
}
