using System;
using System.Net.Sockets;
using Wcs.Plc.Snap7;

namespace dotnet_tcp_client
{
  class Program
  {
    static void Main(string[] args)
    {
      var client = new TcpClient("192.168.20.3", 102);
      var response = new byte[50];
      var stream = client.GetStream();
      var req = new S7Request();
      var res = new S7Response();

      req.Use200Smart();
      req.UseAddress("D1.100", 1);
      req.UseData(true);

      var msg = req.WriteMessage;

      stream.Write(req.CheckHead1, 0, req.CheckHead1.Length);
      stream.Read(response, 0, response.Length);

      stream.Write(req.CheckHead2, 0, req.CheckHead2.Length);
      stream.Read(response, 0, response.Length);

      try {
        stream.Write(msg, 0, msg.Length);
        stream.Read(response, 0, response.Length);
      } catch {
        Console.WriteLine("error");
      }

      res.SetMessage(response);
      Console.WriteLine(res.DataCode);

      Console.WriteLine("Write request: " + BitConverter.ToString(msg));
      Console.WriteLine("Write response: " + BitConverter.ToString(response));

      Console.WriteLine(req.ReadMessage);
      req.ReadMessage[27] = 0x99;
      stream.Write(req.ReadMessage, 0, req.ReadMessage.Length);
      stream.Read(response, 0, 50);

      res.SetMessage(response);
      Console.WriteLine("Read Request: " + BitConverter.ToString(req.ReadMessage));
      Console.WriteLine("Read Response: " + BitConverter.ToString(response));

      stream.Close();
      client.Close();
    }
  }
}
