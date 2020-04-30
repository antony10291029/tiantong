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

      plc.Name("测试200smart").UseMC3E("192.168.20.11", 8000);

      plc.Define("心跳").Int("D100", 1).Heartbeat(100).Collect(100);
      plc.Define("扫码器").String("D120", 10).Collect(100);

      plc.Int("心跳").Watch(value => value > 0).Event("event");
      plc.String("扫码器").Watch(value => value != "000000000").Event("scanning");

      plc.Int("心跳").On("event", value => {
        Console.WriteLine(value);
        plc.String("扫码器").Set(random.String(10));
      });
      plc.String("扫码器").On("scanning", value => {
        Console.WriteLine("扫码数据: " + value);
        // plc.String("扫码器").Set("000000000");
      });

      plc.Run();
    }
  }
}
