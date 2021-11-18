using System;
using System.IO;

namespace System.Net.Sockets
{
  public class RenetTcpClient: IDisposable
  {
    public int Port;

    public string Host;

    private Stream _stream;

    private TcpClient _client;

    private int _ioTimeout = 10000;

    private int BufferLength = 10240;

    private readonly object _sendingLock = new object();

    private readonly object _reconnectLock = new object();

    public RenetTcpClient(string host, int port)
    {
      Host = host;
      Port = port;
    }

    public void Dispose()
    {
      _stream?.Dispose();
      _client?.Dispose();
    }

    public void Close()
    {
      _stream?.Close();
      _client?.Close();
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
      _client.ReceiveBufferSize = _client.SendBufferSize = BufferLength;

      if (_client.ConnectAsync(Host, Port).Wait(_ioTimeout)) {
        _stream = _client.GetStream();
        _stream.ReadTimeout = _stream.WriteTimeout = _ioTimeout;
        Connected();
      } else {
        throw KnownException.Error("网络连接超时");
      }
    }

    public virtual void Connected()
    {

    }
  }
}
