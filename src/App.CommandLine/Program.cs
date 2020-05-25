using System;
using Tiantong.Iot;
using Tiantong.Iot.Entities;

namespace App.CommandLine
{
  static class Program
  {
    static void Main()
    {
      var worker = new PlcWorker();

      worker.Config(configer => {
        configer.Name("测试").Host("192.168.3.39").Port(8001).Model(PlcModel.MC3E);
      });

      worker.Define("心跳").UInt16("D800");

      worker.Heartbeat("心跳", 1000, 10000);

      worker.Define("扫码器").String("D2011", 10, builder => {
        builder.Collect(1000).Watch(value => {
          Console.WriteLine("扫码器: " + value);
        });
      });

      worker.Run();

      // Task.Delay(1000000).GetAwaiter().GetResult();
    }
  }
}
