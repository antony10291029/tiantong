using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Midos.Domain;

namespace Midos.Services.Http
{
  public interface IHttpService
  {
    string Post<TRequest>(string url, TRequest data);

    TResponse Post<TRequest, TResponse>(string url, TRequest data);

    Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data);
  }

  public class HttpService: IHttpService
  {
    private readonly HttpClient _client;

    private readonly IEventPublisher _publisher;

    public HttpService(
      IHttpClientFactory factory,
      IEventPublisher publisher
    ) {
      _client = factory.CreateClient();
      _publisher = publisher;
    }

    public string Post<TRequest>(string url, TRequest data)
    {
      string result;

      try {
        result = _client.Post<TRequest>(url, data);

        _publisher.Publish(
          HttpLogEvent.@event,
          HttpLogEvent.From(
            method: "post",
            url: url,
            status: 200,
            request: data,
            response: JsonSerializer.Deserialize<object>(result)
          )
        );
      } catch (HttpPostException ex) {
        _publisher.Publish(
          HttpLogEvent.@event,
          HttpLogEvent.From(
            method: "post",
            url: url,
            status: ex.Status,
            request: data,
            response: JsonSerializer.Deserialize<object>(ex.Body)
          )
        );

        throw;
      }

      return result;
    }

    public TResponse Post<TRequest, TResponse>(string url, TRequest data)
    {
      TResponse result;

      try {
        result = _client.Post<TRequest, TResponse>(url, data);

        _publisher.Publish(
          HttpLogEvent.@event,
          HttpLogEvent.From(
            method: "post",
            url: url,
            status: 200,
            request: data,
            response: result
          )
        );
      } catch (HttpPostException ex) {
        _publisher.Publish(
          HttpLogEvent.@event,
          HttpLogEvent.From(
            method: "post",
            url: url,
            status: ex.Status,
            request: data,
            response: JsonSerializer.Deserialize<object>(ex.Body)
          )
        );

        throw;
      }

      return result;
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data)
    {
      TResponse result;

      try {
        result = await _client.PostAsync<TRequest, TResponse>(url, data);
        _publisher.Publish(
          HttpLogEvent.@event,
          HttpLogEvent.From(
            method: "post",
            url: url,
            status: 200,
            request: data,
            response: result
          )
        );
      } catch (HttpPostException ex) {
        _publisher.Publish(
          HttpLogEvent.@event,
          HttpLogEvent.From(
            method: "post",
            url: url,
            status: ex.Status,
            request: data,
            response: JsonSerializer.Deserialize<object>(ex.Body)
          )
        );

        throw;
      }

      return result;
    }
  }
}
