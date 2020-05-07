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

      plc.Name("测试200smart").UseS7200Smart("127.0.0.1", 102);

      plc.State("心跳").Int("D1.100").Heartbeat(1000).Collect(1000);
      plc.State("扫码器").String("D1.120", 10).Collect(500);

      plc.Int("心跳").Watch(value => value > 0).Event("event");
      plc.String("扫码器").Watch(value => value != "000000000").Event("scanning");

      plc.Int("心跳").On("event", value => {
        Console.WriteLine(value);
        plc.String("扫码器").Set(random.String(9));
      });
      plc.String("扫码器").On("scanning", value => {
        Console.WriteLine("收到数据: " + value);
        plc.String("扫码器").Set("000000000");
      });

      plc.Run();
    }
  }
}
