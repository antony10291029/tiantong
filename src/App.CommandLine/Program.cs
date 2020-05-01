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

      plc.Name("测试200smart").UseS7200Smart("192.168.20.10", 102);

      plc.Define("心跳").UShort("D1.100").Heartbeat(1000).Collect(1000);
      plc.Define("扫码器").String("D1.120", 10).Collect(1000);

      plc.UShort("心跳").Watch(value => value > 0).Event("触发心跳");
      plc.String("扫码器").Watch("!=", "000000000").Event("触发扫码");

      plc.UShort("心跳").On("触发心跳", value => {
        Console.WriteLine(value);
        plc.String("扫码器").Set(random.String(10));
      });
      plc.String("扫码器").On("触发扫码", value => {
        Console.WriteLine("扫码数据: " + value);
        plc.String("扫码器").Set("000000000");
      });

      plc.Run();
    }
  }
}
