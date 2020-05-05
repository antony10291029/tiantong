using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Tiantong.Iot.Test
{
  public class TestWatcherHttpListener
  {
    private HttpListener _listener = new HttpListener();

    private Task _task;

    public TestWatcherHttpListener()
    {

    }

    public string Start()
    {
      var url = $"http://localhost:{Port.Free()}/";

      _listener.Prefixes.Add(url);
      _listener.Start();
      var task = _listener.GetContextAsync();
      _task = task.ContinueWith(t => {
        var context = t.GetAwaiter().GetResult();
        var response = context.Response;

        response.StatusCode = 200;
        var buffer = Encoding.UTF8.GetBytes("<<<testing watcher http server>>>");
        response.OutputStream.Write(buffer, 0, buffer.Length);
        response.OutputStream.Close();
        _listener.Close();
      });

      Console.WriteLine($"http listener is starting on {url}");

      return url;
    }

    public void Wait()
    {
      _task?.GetAwaiter().GetResult();
    }
  }
}
