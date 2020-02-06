using System;
using Wcs.Plc;

namespace App.CommandLine
{
  class Program
  {
    static void Main()
    {
      var plc = new Plc();

      plc.State("hb").Word("D1").Heartbeat(1).Collect(1);
      plc.State("scanner").Words("D2").Collect(1);

      plc.Word("hb").Watch(value => value > 0).Event("event");
      plc.Words("scanner").Watch(value => value != null).Event("scanning");

      plc.On<int>("event", value => Console.WriteLine(value));
      plc.On<string>("scanning", value => {});

      plc.Words("scanner").Set("ojbk");

      plc.Run();
    }
  }
}
