using System.ComponentModel;
using System.Text;
using System;
using Renet.Tcp;
using Wcs.Plc.Melsec;

namespace App.CommandLine
{
  class Program
  {
    static void Main()
    {
      var client = new MC3ETcpClient("127.0.0.1", 6000);
      var request = new MC3ERequest();
      var response = new MC3EResponse();

      request.UseAddress("D100", 4);

      client.Read(request, response);

      Console.WriteLine(BitConverter.ToString(response.Response));
    }
  }
}
