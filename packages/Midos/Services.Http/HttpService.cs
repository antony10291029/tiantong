using System.Net.Http;
using System.Threading.Tasks;

namespace Midos.Services.Http
{
  public interface IHttpService
  {
    TResponse Post<TRequest, TResponse>(string url, TRequest data);

    Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data);
  }

  public class HttpService: IHttpService
  {
    private HttpClient _client;

    public HttpService(IHttpClientFactory factory)
    {
      _client = factory.CreateClient();
    }

    public TResponse Post<TRequest, TResponse>(string url, TRequest data)
      => _client.Post<TRequest, TResponse>(url, data);

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data)
      => await _client.PostAsync<TRequest, TResponse>(url, data);
  }
}
