using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotNetCore.CAP;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

namespace Namei.Wcs.Aggregates
{
  public class WebHookController: BaseController
  {
    private const string Group = "WebHookController";

    private HttpClient _client;

    public WebHookController(IHttpClientFactory factory)
    {
      _client = factory.CreateClient();
      _client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json)
      );
    }

    [CapSubscribe(WebHookPost.Message, Group = Group)]
    public async Task PostAsync(WebHookPost param)
    {
      var content = new StringContent(
        content: param.Data,
        encoding: Encoding.UTF8,
        MediaTypeNames.Application.Json
      );

      await _client.PostAsync(param.Url, content);
    }
  }
}
