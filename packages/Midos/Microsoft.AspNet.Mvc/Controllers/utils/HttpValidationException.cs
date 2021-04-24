using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Mvc
{
  public class HttpValidationException : Exception, IHttpException
  {
    public string Error = "HttpValidationException";

    public int Status { get; set; } = 422;

    protected string _message { get; set; }

    public Dictionary<string, string[]> Details { get; } = new Dictionary<string, string[]>();

    public string Body
    {
      get => JsonSerializer.Serialize(new {
        error = Error,
        message = _message,
      });
    }
  }
}
