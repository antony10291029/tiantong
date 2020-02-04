using System;
using System.Text.Json;
using System.Collections.Generic;

namespace Renet.Web
{
  public class HttpValidationException : Exception, IHttpException
  {
    public string Error = "HttpValidationException";

    public int Status { get; set; } = 422;

    public Dictionary<string, string[]> Details { get; } = new Dictionary<string, string[]>();

    public string Body
    {
      get => JsonSerializer.Serialize(new {
        error = Error,
        message = "fail to verify request data",
        details = Details,
      });
    }

    public void AddDetails(string key, params string[] messages)
    {
      Details.Add(key, messages);
    }
  }
}
