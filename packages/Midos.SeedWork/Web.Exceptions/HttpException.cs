using System;
using System.Text.Json;

namespace Microsoft.AspNetCore.Http
{
  public class HttpException : Exception, IHttpException
  {
    private readonly string _error;

    public int Status { get; set; }

    public string Name
    {
      get => _error ?? this.GetType().Name;
    }

    public string Msg { get; set; }

    public string Body
    {
      get => JsonSerializer.Serialize(new {
        error = Name,
        message = Msg,
      });
    }

    public HttpException(string message, int status = 400, string error = "HttpException")
    {
      Msg = message;
      Status = status;
      _error = error;
    }
  }
}
