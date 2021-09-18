namespace System.Net.Sockets
{
  public class RenetTcpClient: IDisposable
  {
    public int Port;

    public string Host;

    private TcpClient _client;

    private int _ioTimeout = 10000;

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
      _client.Dispose();
    }

    public void Close()
    {
      _client.Close();
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
        if (_client.GetStream().CanWrite) {
          _client.GetStream().Write(message, 0, message.Length);
        }
        if (_client.GetStream().CanRead) {
          _client.GetStream().Read(buffer, 0, buffer.Length);
        }
      }
    }

    public void Connect()
    {
      _client = new TcpClient();
      _client.SendBufferSize = 5120;
      _client.ReceiveBufferSize = 5120;
      _client.SendTimeout = _client.ReceiveTimeout = _ioTimeout;

      if (_client.ConnectAsync(Host, Port).Wait(_ioTimeout)) {
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
