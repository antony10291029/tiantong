using System;
using Wcs.Plc;

namespace App.CommandLine
{
  class Program
  {
    static void Main()
    {
      var plc = new Plc();

      plc.Name("测试200smart").UseS7200Smart("192.168.20.3", 102);

      plc.State("hb").Int("D1.100").Heartbeat(1000).Collect(1000);
      plc.State("scanner").String("D1.120", 10).Collect(1000);

      plc.Int("hb").Watch(value => value > 0).Event("event");
      plc.String("scanner").Watch(value => value != "").Event("scanning");

      plc.Int("hb").On("event", value => Console.WriteLine(value));
      plc.String("scanner").On("scanning", value => {
        Console.WriteLine("收到数据: " + value);
        plc.String("scanner").Set("");
      });

      plc.String("scanner").Set("ojbk");

      plc.Run();
    }
  }
}
