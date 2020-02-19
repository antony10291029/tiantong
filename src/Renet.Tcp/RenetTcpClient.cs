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

    private int BufferLength = 1024;

    private int ReconnectInterval = 1000;

    private readonly object _sandLock = new {};

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

      SendMessage(message, buffer);

      return buffer;
    }

    public void SendMessage(byte[] message, byte[] buffer)
    {
      lock (_sandLock) {
        _stream.Write(message, 0, message.Length);
        _stream.Read(buffer, 0, buffer.Length);
      }
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

    public virtual bool Connect(int time = 0)
    {
      return true;
    }

    public void Reconnect()
    {
      Console.WriteLine("连接已断开，正在重连...");
      for (var i = 0; ; i++) {
        Console.WriteLine("重连中: " + i);
        if (Connect(i)) {
          break;
        }
        Task.Delay(ReconnectInterval);
      };
    }

    protected virtual bool HandleError(Exception exception)
    {
      Console.WriteLine(exception);

      return true;
    }
  }
}
