using System;
using System.IO;
using System.Net.Sockets;

namespace Renet.Tcp
{
  public class RenetTcpClient: IDisposable
  {
    public int Port;

    public string Host;

    private Stream _stream;

    private TcpClient _client;

    private int _ioTimeout = 3000;

    private int BufferLength = 1024;

    private readonly object _sendingLock = new object();

    private readonly object _reconnectLock = new object();

    public RenetTcpClient(string host, int port)
    {
      Host = host;
      Port = port;
    }

    public void Dispose()
    {
      _client?.Dispose();
      _stream?.Dispose();
    }

    public void Close()
    {
      _client?.Close();
      _stream?.Close();
    }

    public byte[] Send(byte[] message)
    {
      var buffer = new byte[BufferLength];

      Send(message, buffer);

      return buffer;
    }

    private void Send(byte[] message, byte[] buffer)
    {
      lock (_sendingLock) {
        if (_stream.CanWrite) {
          _stream.Write(message, 0, message.Length);
        }
        if (_stream.CanRead) {
          _stream.Read(buffer, 0, buffer.Length);
        }
      }
    }

    public void Connect()
    {
      _client = new TcpClient();
      _client.SendBufferSize = 512;
      _client.ReceiveBufferSize = 512;
      if (_client.ConnectAsync(Host, Port).Wait(_ioTimeout)) {
        _stream = _client.GetStream();
        _stream.ReadTimeout = _stream.WriteTimeout = _ioTimeout;
        Connected();
      } else {
        throw new Exception("tcp connect timeout");
      }
    }

    public virtual void Connected()
    {

    }

  }
}
