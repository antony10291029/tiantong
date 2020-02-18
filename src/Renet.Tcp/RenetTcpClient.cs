using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Renet.Tcp
{
  public class RenetTcpClient
  {
    private string _host;

    private int _port;

    private Stream _stream;

    private TcpClient _client;

    public Func<int, bool> Connected;

    private int BufferLength = 1024;

    private int ReconnectInterval = 1000;

    public RenetTcpClient(string host, int port)
    {
      _host = host;
      _port = port;
      _client = new TcpClient(_host, _port);
      _stream = _client.GetStream();
    }

    public void Close()
    {
      _stream.Close();
      _client.Close();
    }

    public byte[] Send(byte[] message)
    {
      var buffer = new byte[BufferLength];

      _stream.Write(message, 0, message.Length);
      _stream.Read(buffer, 0, buffer.Length);

      return buffer;
    }

    public byte[] TrySend(byte[] message)
    {
      try {
        return Send(message);
      } catch (Exception ex) {
        HandleError(ex);
        Reconnect();
        return TrySend(message);
      }
    }

    public void SendMessage(byte[] message, byte[] buffer)
    {

    }

    public bool Connect(int time = 0)
    {
      if (Connected != null) {
        return Connected(time);
      } else {
        return true;
      }
    }

    public void Reconnect()
    {
      for (var i = 0; ; i++) {
        Console.WriteLine("重连中: " + i);
        if (Connected(i)) {
          break;
        }
        Task.Delay(ReconnectInterval);
      };
    }

    private void HandleSend(byte[] message, byte[] buffer)
    {
      var str = BitConverter.ToString(message);

      Console.WriteLine(str);

      while (true) {
        try {
          SendMessage(message, buffer);
        } catch (Exception e) {
          if (HandleError(e)) {
            Task.Delay(ReconnectInterval).GetAwaiter().GetResult();
            for (var i = 0; Connect(i); i++);
          } else {
            break;
          }
        }
      }
    }

    protected virtual bool HandleError(Exception exception)
    {
      Console.WriteLine(exception);

      return true;
    }
  }
}
