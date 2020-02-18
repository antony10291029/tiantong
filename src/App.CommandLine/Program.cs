using System;
using Wcs.Plc;

namespace App.CommandLine
{
  class Program
  {
    static void Main()
    {
      var plc = new Plc();

      plc.State("hb").Int("D1").Heartbeat(1).Collect(1);
      plc.State("scanner").String("D2").Collect(1);

      plc.Int("hb").Watch(value => value > 0).Event("event");
      plc.String("scanner").Watch(value => value != null).Event("scanning");

      plc.Int("hb").On("event", value => Console.WriteLine(value));
      plc.String("scanner").On("scanning", value => {});

      plc.String("scanner").Set("ojbk");

      plc.Run();
    }
  }
}
