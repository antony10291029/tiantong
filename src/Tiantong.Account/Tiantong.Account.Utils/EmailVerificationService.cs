using Renet;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;

namespace Tiantong.Account.Utils
{
  public class EmailVerificationService
  {
    private HttpClient _client;

    public EmailVerificationService(HttpClient client)
    {
      _client = client;
      _client.BaseAddress = new Uri("https://iaccount.als-yuchuan.com");
      _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
    }

    public async Task<string> SendAsync(string address, string subject = "邮箱认证", int duration = 300)
    {
      var text = JsonSerializer.Serialize(new {
        address,
        subject,
        duration
      });
      var content = new StringContent(text, Encoding.UTF8, MediaTypeNames.Application.Json);
      var response = await _client.PostAsync("/email-verification/send", content);
      var dom = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

      if (response.StatusCode == HttpStatusCode.OK) {

        return dom.RootElement.GetProperty("key").GetString();
      } else {
        var message = dom.RootElement.GetProperty("message").GetString();

        throw KnownException.Error(message, (int) response.StatusCode);
      }
    }

    public async Task VerifyAsync(string address, string key, string code)
    {
      var text = JsonSerializer.Serialize(new {
        address,
        key,
        code
      });

      var content = new StringContent(text, Encoding.UTF8, MediaTypeNames.Application.Json);
      var response = await _client.PostAsync("/email-verification/verify", content);
      var dom = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

      if (response.StatusCode != HttpStatusCode.OK) {
        throw KnownException.Error(
          dom.RootElement.GetProperty("message").GetString(),
          (int) response.StatusCode
        );
      }
    }
  }
}
