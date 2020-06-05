using System;
using Renet.Net;
using Tiantong.Iot.Protocol;

namespace App.Melsec.CommandLine
{
  public static class Program
  {
    public static void Main()
    {
      var client = new RenetTcpClient("192.168.20.10", 8000);
      var readRequest = new MC3EBinaryReadRequest();
      var readResponse = new MC3EBinaryReadResponse();
      var writeRequest = new MC3EBinaryWriteRequest();
      var writeResponse = new MC3EBinaryWriteResponse();

      client.Connect();

      writeRequest.UseUInt16();
      writeResponse.UseUInt16();
      readRequest.UseUInt16();
      readResponse.UseUInt16();

      writeRequest.UseAddress("M100");
      readRequest.UseAddress("M100");

      writeRequest.UseData(1000);

      writeResponse.Message = client.Send(writeRequest.Message);
      readResponse.Message = client.Send(readRequest.Message);

      Console.WriteLine(readResponse.GetUInt16());

      client.Close();
    }
  }
}
