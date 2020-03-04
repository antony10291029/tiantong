using System;
using System.Text.Json;

namespace Renet.Web
{
  public class HttpException : Exception, IHttpException
  {
    private string _error;

    public int Status { get; set; }

    public string Name
    {
      get => _error == null ? this.GetType().Name : _error;
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
