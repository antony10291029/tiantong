using System;
using System.Text.Json;

namespace Renet.Web
{
  public class HttpException : Exception, IHttpException
  {
    public int Status { get; set; }

    public string Name
    {
      get => this.GetType().Name;
    }

    public string Msg { get; set; }

    public string Body
    {
      get => JsonSerializer.Serialize(new {
        error = Name,
        message = Msg,
      });
    }

    public HttpException(string message, int status = 400)
    {
      Msg = message;
      Status = status;
    }
  }
}
