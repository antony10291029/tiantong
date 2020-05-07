using System.ComponentModel;
using System.Text;
using System;
using Renet.Tcp;
using Wcs.Plc.Protocol;

namespace App.CommandLine
{
  class Program
  {
    static void Main()
    {
      var client = new RenetTcpClient("192.168.20.11", 8000);
      client.Connect();

      HandleWrite(client);
      HandleRead(client);
    }

    static void HandleWrite(RenetTcpClient client)
    {
      var request = new MC3EWriteRequest();
      var response = new MC3EWriteResponse();

      request.UseAddress("M100", 1);
      request.UseData(100);

      Console.WriteLine(
        BitConverter.ToString(request.Message)
      );
      response.Message = client.TrySend(request.Message);

      Console.WriteLine(response.IsError);
    }

    static void HandleRead(RenetTcpClient client)
    {
      var request = new MC3EReadRequest();
      var response = new MC3EReadResponse();

      request.UseAddress("M100", 1);

      Console.WriteLine(BitConverter.ToString(request.Message));

      response.Message = client.TrySend(request.Message);
      Console.WriteLine(BitConverter.ToString(response.ResultCode));
      Console.WriteLine(BitConverter.ToString(response.Data));
      Console.WriteLine(response.GetInt32());
    }
  }
}
