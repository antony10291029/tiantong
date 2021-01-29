using System;
using System.Text.Json;

namespace Renet.Web
{
  public class SuccessOperation : Exception, IHttpException
  {
    public int Status { get; set; } = 201;

    public string Body
    {
      get => JsonSerializer.Serialize(_body);
    }

    private object _body;

    public SuccessOperation(string message)
    {
      _body = new { message };
    }

    public SuccessOperation(object body)
    {
      _body = body;
    }
  }
}
