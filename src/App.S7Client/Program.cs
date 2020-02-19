using System;
using System.Threading.Tasks;
using Wcs.Plc.Snap7;

namespace App.Snap7
{
  class Program
  {
    static void Main(string[] args)
    {
      var req = new S7Request();
      var res = new S7Response();
      var client = new S7TcpClient("192.168.20.3", 102);

      client.Use200Smart();
      client.Connect();

      req.UseAddress("D1.100", 4);
      req.UseData(5999);

      client.Write(req, res);
      Console.WriteLine(res.DataCode);

      for (var i = 0; i < 10000; i++) {
        client.Read(req, res);
        Console.WriteLine($"{i}: {res.ToInt32()}");
        Task.Delay(1000).GetAwaiter().GetResult();
      }

      client.Close();
    }
  }
}
