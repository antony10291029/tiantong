using System;
using System.Text.Json;

namespace Renet.Web.Example
{
  public class HttpExampleException : Exception, IHttpException
  {
    public int Status { get; set; } = 500;

    public string Body
    {
      get => JsonSerializer.Serialize(new {
        error = "HttpExampleException",
        message = "this error was thown manually"
      });
    }
  }
}
