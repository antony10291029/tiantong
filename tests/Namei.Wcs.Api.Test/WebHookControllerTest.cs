using System.Text;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Midos.Domain.Test;
using Namei.Wcs.Aggregates;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text.Json;

namespace Namei.Wcs.Api.Test
{
  public class TestHttpFactory: IHttpClientFactory
  {
    public HttpClient CreateClient(string name)
      => new HttpClient();
  }

  [TestClass]
  public class WebHookControllerTest
  {
    public static int FreePort()
    {
      var server = new TcpListener(IPAddress.Loopback, 0);
      server.Start();
      var port = ((IPEndPoint)server.LocalEndpoint).Port;
      server.Stop();

      return port;
    }

    [TestMethod]
    public void TestPost()
    {
      var port = FreePort();
      var server = new HttpListener();
      var factory = new TestHttpFactory();
      var controller = new WebHookController(factory);
      var data = new { Id = "100", Name = "name" };
      var uri = $"http://localhost:{port}/";
      var url = $"http://localhost:{port}/test";
      var param = new WebHookPost(
        url: url,
        data: JsonSerializer.Serialize(data)
      );

      server.Prefixes.Add(uri);
      server.Start();

      _ = controller.PostAsync(param);
      var context = server.GetContext();
      var request = context.Request;
      var requestContent = "";

      using (var stream = request.InputStream) {
        using (var reader = new StreamReader(stream, Encoding.UTF8)) {
          requestContent = reader.ReadToEnd();
        }
      }

      var response = context.Response;
      var responseContent = JsonSerializer.Serialize(new { message = "OK" });
      var buffer = Encoding.UTF8.GetBytes(responseContent);

      response.ContentLength64 = buffer.Length;

      var output = response.OutputStream;

      output.Write(buffer, 0, buffer.Length);
      output.Close();

      server.Stop();

      Assert.AreEqual(request.Url, param.Url);
      Assert.AreEqual(requestContent, param.Data);
    }
  }
}
