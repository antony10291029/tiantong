using System;
using Wcs.Plc;

namespace App.CommandLine
{
  class Program
  {
    static void Main()
    {
      var plc = new Plc();

      plc.UseS7200Smart();
      plc.State("hb").Int("D1.100").Heartbeat(1).Collect(1);
      plc.State("scanner").String("D1.120", 10).Collect(1);

      plc.Int("hb").Watch(value => value > 0).Event("event");
      plc.String("scanner").Watch(value => value != null).Event("scanning");

      plc.Int("hb").On("event", value => Console.WriteLine(value));
      plc.String("scanner").On("scanning", value => Console.WriteLine("scanning: " + value));

      plc.String("scanner").Set("ojbk");

      plc.Run();
    }
  }
}
