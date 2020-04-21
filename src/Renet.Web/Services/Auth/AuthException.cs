using System;
using System.Text.Json;

namespace Renet.Web
{
  public class AuthException : Exception, IHttpException
  {
    private string _msg;

    public int Status { get => 401; }

    public string Body {
      get => JsonSerializer.Serialize(new {
        error = "AuthException",
        message = _msg
      });
    }

    public AuthException(string msg)
    {
      _msg = msg;
    }
  }
}
