using Microsoft.VisualStudio.TestTools.UnitTesting;
using Midos.Test;

namespace System.Net.Http.Test
{
  [TestClass]
  public class HttpClientTest
  {
    public record Request
    {
      public int Id { get; set; } = 100;

      public string Name { get; set; } = "Foo";
    }

    public record Response
    {
      public int Status { get; set; } = 200;

      public string Message = "success";
    }

    [TestMethod]
    [DataRow(200)]
    [DataRow(400)]
    public void Test_Post(int status)
    {
      var port = TestUtils.GetFreePort();
      var url = $"http://localhost:{port}/";
      var client = new HttpClient();
      var listener = new HttpListener();
      var request = new Request();
      var response = new Response();

      listener.Start(url);

      var task = client.PostAsync<Request, Response>($"http://localhost:{port}/test", request);

      listener.Handle<Request, Response>(
        data => {
          Assert.AreEqual(data, request);
          return new Response();
        },
        status
      );
      listener.Close();

      if (status < 400) {
        Assert.AreEqual(task.GetAwaiter().GetResult(), response);
      } else {
        try {
          task.GetAwaiter().GetResult();
          Assert.Fail("expect an exception when status code >= 400");
        } catch {}
      }
    }
  }
}
