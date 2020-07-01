using Renet;
using System;
using System.Net;
using System.Net.Mime;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tiantong.Account.Utils
{
  public class TokenService
  {
    private HttpClient _client;

    public TokenService(HttpClient client)
    {
      _client = client;
      _client.BaseAddress = new Uri("https://iaccount.als-yuchuan.com");
      _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
    }

    public async Task<int> VerifyAsync(string token)
    {
      if (token == null) {
        throw KnownException.Error("请提供身份凭证", 401);
      }

      var content = new StringContent("{}", Encoding.UTF8, MediaTypeNames.Application.Json);
      _client.DefaultRequestHeaders.Add("Authorization", token);

      var response = await _client.PostAsync("/token/verify", content);
      var dom = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

      if (response.StatusCode != HttpStatusCode.OK) {
        throw KnownException.Error(dom.RootElement.GetProperty("message").GetString(), (int) response.StatusCode);
      }

      return dom.RootElement.GetProperty("id").GetInt32();
    }
  }
}
