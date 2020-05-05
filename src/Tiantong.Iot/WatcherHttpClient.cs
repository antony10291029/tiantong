using System;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public class WatcherHttpClient: IWatcherHttpClient
  {
    private HttpClient _client = new HttpClient();

    private readonly object _sendLock = new object();

    private DbContext _db;

    public WatcherHttpClient(DbContext _db)
    {
      _client.DefaultRequestHeaders
        .Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public void Post(string uri, string data)
    {
      var content = new StringContent(data, Encoding.UTF8, "application/json");
      var task = _client.PostAsync(uri, content);

      var response = task.GetAwaiter().GetResult();
      Console.WriteLine(response.StatusCode.ToString());
      Console.WriteLine(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
    }
  }
}
