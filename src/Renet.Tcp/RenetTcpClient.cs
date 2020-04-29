using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Renet.Tcp
{
  public class RenetTcpClient: IDisposable
  {
    public int Port;

    public string Host;

    private Stream _stream;

    private TcpClient _client;

    private Task _reconnectTask = null;

    private int _ioTimeout = 1000;

    private int BufferLength = 1024;

    private int ReconnectInterval = 1000;

    private readonly object _sendingLock = new object();

    private readonly object _reconnectLock = new object();

    public RenetTcpClient(string host, int port)
    {
      Host = host;
      Port = port;
      _client = new TcpClient();
    }

    ~RenetTcpClient()
    {
      Dispose();
    }

    public void Dispose()
    {
      if (_client != null) {
        _client.Dispose();
      }
      if (_stream != null) {
        _stream.Dispose();
      }
    }

    public void Close()
    {
      if (_client != null) {
        _client.Close();
      }
      if (_stream != null) {
        _stream.Close();
      }
    }

    public byte[] Send(byte[] message)
    {
      var buffer = new byte[BufferLength];

      Send(message, buffer);

      return buffer;
    }

    public void Send(byte[] message, byte[] buffer)
    {
      lock (_sendingLock) {
        _stream.Write(message, 0, message.Length);
        _stream.Read(buffer, 0, buffer.Length);
      }
    }

    public byte[] TrySend(byte[] message)
    {
      while (true) {
        try {
          return Send(message);
        } catch (Exception e) {
          HandleError(e);
          Reconnect();
        }
      }
    }

    public void Connect()
    {
      try {
        Console.WriteLine("正在建立连接");
        var tokenSource = new CancellationTokenSource();
        var task = _client.ConnectAsync(Host, Port)
          .ContinueWith(_ => {
            _.GetAwaiter().GetResult();
            _stream = _client.GetStream();
            _stream.WriteTimeout = _stream.ReadTimeout = _ioTimeout;
            tokenSource.Cancel();
            Connected();
          });
        Task.Delay(_ioTimeout, tokenSource.Token).GetAwaiter().GetResult();
      } catch (TaskCanceledException) {
      } catch (Exception e) {

        throw e;
      }
    }

    public virtual void Connected()
    {

    }

    private async Task ReconnectAsync()
    {
      for (var i = 0; ; i++) {
        try {
          Connect();
          break;
        } catch (Exception e) {
          Console.WriteLine(e.Message);
          Console.WriteLine("正在重连: " + i);
          await Task.Delay(ReconnectInterval);
        }
      };
    }

    public void Reconnect()
    {
      lock (_reconnectLock) {
        if (_reconnectTask == null) {
          _reconnectTask = ReconnectAsync();
        }
      }

      _reconnectTask.GetAwaiter().GetResult();
    }

    protected virtual bool HandleError(Exception exception)
    {
      Console.WriteLine(exception);

      return true;
    }
  }
}
