using System;
using System.Threading.Tasks;
using Wcs.Plc.Snap7;
using Renet.Tcp;

namespace dotnet_tcp_client
{
  class Program
  {
    static void Main(string[] args)
    {
      var client = new RenetTcpClient("192.168.20.3", 102);
      var req = new S7Request();
      var res = new S7Response();

      client.Connected = _ => {
        try {
          var msg = client.Send(req.CheckHead1);
          msg = client.Send(req.CheckHead2);

          return false;
        } catch {
          return true;
        }
      };

      req.Use200Smart();
      req.UseAddress("D1.100", 4);
      req.UseData(2000);

      client.Connect();

      var data = client.TrySend(req.WriteMessage);
      res.SetMessage(data);
      Console.WriteLine(res.DataCode);

      for (var i = 0; i < 10000; i++) {
        data = client.TrySend(req.ReadMessage);
        res.SetMessage(data);
        Console.WriteLine($"{i}: {res.ToInt32()}");
        Task.Delay(1000).GetAwaiter().GetResult();
      }

      client.Close();
    }
  }
}
