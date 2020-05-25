using System;
using Tiantong.Iot;
using Tiantong.Iot.Entities;
using Tiantong.Iot.DB.Sqlite;

namespace App.CommandLine
{
  static class Program
  {
    static void Main()
    {
      using (var db = new IotSqliteDbcontext()) {
        var mg = new IotSqliteMigrator(db);
        mg.Migrate();
      }

      var worker = new PlcWorker();

      worker.Config(configer => {
        configer.Name("测试").Host("192.168.3.39").Port(8001).Model(PlcModel.Test);
      });

      worker.Define("心跳").UInt16("D800");

      worker.Heartbeat("心跳", 1000, 10000);

      worker.Define("扫码器").String("D2011", 10);

      worker.Collect<ushort>("心跳", 1);
      worker.Watch("心跳", value => Console.WriteLine(value));

      worker.Run();
    }

  }

}
