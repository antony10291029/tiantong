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
  public class PasswordService
  {
    private HttpClient _client;

    public PasswordService(HttpClient client)
    {
      _client = client;
      _client.BaseAddress = new Uri("https://iaccount.als-yuchuan.com");
      _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
    }

    public async Task ConfirmAsync(string token, string password)
    {
      if (password == null) {
        throw KnownException.Error("密码不能为空");
      }

      var text = JsonSerializer.Serialize(new {
        password = password
      });

      var content = new StringContent(text, Encoding.UTF8, MediaTypeNames.Application.Json);

      using (var request = new HttpRequestMessage(HttpMethod.Post, "/password/confirm")) {
        request.Content = content;
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);

        var dom = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        if (response.StatusCode != HttpStatusCode.OK) {
          throw KnownException.Error(dom.RootElement.GetProperty("message").GetString(), (int) response.StatusCode);
        }
      }
    }
  }
}
