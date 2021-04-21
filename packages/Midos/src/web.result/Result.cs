using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc
{
  public class Result<T>: ObjectResult, IResult<T>
  {
    protected Dictionary<string, string> _headers = new Dictionary<string, string>();

    public Result(object data): base(data) {}

    public int? GetStatusCode() => base.StatusCode;

    public string GetHeader(string key) => _headers[key];

    public IResult<T> Header(string key, string value)
    {
      _headers.Add(key, value);

      return this;
    }

    public new IResult<T> StatusCode(int code)
    {
      base.StatusCode = code;

      return this;
    }

    public override Task ExecuteResultAsync(ActionContext context)
    {
      foreach (var header in _headers) {
        context.HttpContext.Response.Headers.Add(header.Key, header.Value);
      }

      return base.ExecuteResultAsync(context);
    }
  }
}
