using System;
using Renet.Tcp;

namespace Wcs.Plc.Melsec
{
  public class MC3ETcpClient : RenetTcpClient
  {
    public MC3ETcpClient(string host, int port): base(host, port)
    {

    }

    public void Read(MC3ERequest request, MC3EResponse response)
    {
      var message = TrySend(request.ReadMessage);
      response.SetMessage(message);
    }

    public void Write(MC3ERequest request, MC3EResponse response)
    {
      var message = TrySend(request.WriteMessage);
      response.SetMessage(message);
    }
  }
}
