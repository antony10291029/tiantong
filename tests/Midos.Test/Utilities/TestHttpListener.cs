using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Midos.Test
{
  public static class HttpListenerExtensions
  {
    public static void Start(this HttpListener listener, string url)
    {
      listener.Prefixes.Add(url);
      listener.Start();
    }

    public static void Handle<TRequest, TResponse>(
      this HttpListener listener,
      Func<TRequest, TResponse> handle,
      int statusCode = 200
    ) {
      var context = listener.GetContext();
      var request = context.Request;
      var response = context.Response;

      using var stream = request.InputStream;
      using var reader = new StreamReader(stream, Encoding.UTF8);

      var requestData = JsonSerializer.Deserialize<TRequest>(reader.ReadToEnd());
      var responseData = handle(requestData);
      var responseContent = JsonSerializer.Serialize(responseData);
      var buffer = Encoding.UTF8.GetBytes(responseContent);

      response.StatusCode = statusCode;
      using var output = response.OutputStream;

      output.Write(buffer, 0, buffer.Length);
    }
  }
}
