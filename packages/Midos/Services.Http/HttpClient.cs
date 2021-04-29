using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace System.Net.Http
{
  public class HttpPostException: Exception, IHttpException
  {
    public int Status { get; } 

    public string Body { get; }

    public HttpPostException(int code, string body)
    {
      Status = code;
      Body = body;
    }
  }

  public static class HttpClientExtensions
  {
    public static async Task<TResponse> PostAsync<TRequest, TResponse>(
      this HttpClient client,
      string url,
      TRequest data
    ) => JsonSerializer.Deserialize<TResponse>(await PostAsync(client, url, data));

    public static TResponse Post<TRequest, TResponse>(
      this HttpClient client,
      string url,
      TRequest data
    ) {
      return client
        .PostAsync<TRequest, TResponse>(url, data)
        .GetAwaiter()
        .GetResult();
    }

    public static async Task<String> PostAsync<TRequest>(
      this HttpClient client,
      string url,
      TRequest data
    ) {
      var text = JsonSerializer.Serialize(data);
      var content = new StringContent(
        content: text,
        encoding: Encoding.UTF8,
        mediaType: MediaTypeNames.Application.Json
      );
      var response = await client.PostAsync(url, content);
      var result = await response.Content.ReadAsStringAsync();
      var code = (int) response.StatusCode;

      if (code >= 400) {
        throw new HttpPostException(code, result);
      }

      return result;
    }

    public static String Post<TRequest>(
      this HttpClient client,
      string url,
      TRequest data
    ) => PostAsync(client, url, data).GetAwaiter().GetResult();
  }

}
