using System;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Tiantong.Iot.Api
{
  public class FailureOperation : Exception, IHttpException
  {
    public int Status { get; set; } = 422;

    public string Body
    {
      get => JsonSerializer.Serialize(_body);
    }

    private object _body;

    public FailureOperation(string message)
    {
      _body = new { message };
    }

    public FailureOperation(object body)
    {
      _body = body;
    }
  }
}
