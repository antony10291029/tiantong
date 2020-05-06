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

      plc.Config(configer => {
        configer.Name("测试200smart")
          .Model(PlcModel.S7200Smart)
          .Host("192.168.20.10").Port(102);
      });

      plc.Define("心跳").UShort("D1.100", state => {
        state.Heartbeat(1000).Collect(1000);
        state.Watch(value => {
          Console.WriteLine(value);
          plc.String("扫码器").Set(random.String(10));
        });
      });

      plc.Define("扫码器").String("D1.120", 10, state => {
        state.Collect(1000);
        state.Watch("!=", "0000000000").Id(1, 10, 100)
          .HttpPost("http://localhost:5000/test", "value");
      });

      plc.Run();
    }
  }
}
