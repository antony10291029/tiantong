using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Midos.Domain;
using System;
using System.Net.Mime;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Midos.Services.Http
{
  public class HttpServiceController: BaseController
  {
    private const string Group = "HttpServiceController";

    private readonly HttpClient _client;

    private readonly IEventPublisher _publisher;

    public HttpServiceController(
      IEventPublisher publisher,
      IHttpClientFactory factory
    ) {
      _publisher = publisher;
      _client = factory.CreateClient();
    }

    [CapSubscribe(HttpPost.Event, Group = Group)]
    public async Task Post(HttpPost param)
    {
      var contentString = Encoding.UTF8.GetString(param.Data);

      try {
        var content = new StringContent(
          content: contentString,
          encoding: Encoding.UTF8,
          mediaType: MediaTypeNames.Application.Json
        );
        var response = await _client.PostAsync(param.Url, content);
        var body = await response.Content.ReadAsStringAsync();

        _publisher.Publish(
          HttpLogEvent.@event,
          HttpLogEvent.From(
            method: "post",
            url: param.Url,
            status: (int) response.StatusCode,
            request: contentString,
            response: body
          )
        );
      } catch (Exception e) {
        _publisher.Publish(
          HttpLogEvent.@event,
          HttpLogEvent.From(
            method: "post",
            url: param.Url,
            status: 500,
            request: contentString,
            response: e.Message
          )
        );

        throw;
      }
    }
  }
}
