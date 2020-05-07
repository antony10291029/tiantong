using System;
using Wcs.Plc;

namespace App.CommandLine
{
  class Program
  {
    static void Main()
    {
      var plc = new Plc();
      var random = new Renet.Random();

      plc.Name("测试200smart").UseTest();

      plc.Define("心跳").UShort("D100").Heartbeat(0).Collect(0);
      plc.Define("扫码器").String("D120", 10).Collect(0);

      plc.UShort("心跳").Watch(value => value > 0).Event("event");
      plc.String("扫码器").Watch(value => value != "000000000").Event("scanning");

      plc.UShort("心跳").On("event", value => {
        Console.WriteLine(value);
        // plc.String("扫码器").Set(random.String(10));
      });
      plc.String("扫码器").On("scanning", value => {
        // Console.WriteLine("扫码数据: " + value);
        // plc.String("扫码器").Set("000000000");
      });

      plc.Run();
    }
  }
}
