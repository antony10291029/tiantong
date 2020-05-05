using System;
using Tiantong.Iot;

namespace App.CommandLine
{
  class Program
  {
    static void Main()
    {
      var plc = new PlcWorker();
      var random = new Renet.Random();

      plc.UseTest();
      // plc.Name("测试200smart").UseS7200Smart("192.168.20.10", 102);

      plc.Define("心跳").UShort("D1.100")
        .Heartbeat(1000).Collect(1000)
        .Watch(value => {
          Console.WriteLine(value);
          plc.String("扫码器").Set(random.String(10));
        });

      plc.Define("扫码器").String("D1.120", 10).Collect(1000)
        .Watch("!=", "0000000000").HttpPost("http://localhost:5000/test", "value");

      plc.Run();
    }
  }
}
