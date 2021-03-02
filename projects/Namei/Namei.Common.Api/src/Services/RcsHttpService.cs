using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Namei.Common.Api
{
  public class RcsHttpService
  {
    private HttpClient _client;

    public RcsHttpService(Config _config, IHttpClientFactory factory)
    {
      _client = factory.CreateClient();
      _client.Timeout = new TimeSpan(0, 0, 10);
      _client.BaseAddress = new Uri(_config.RcsUrl);
      _client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json)
      );
    }

    public async Task<BindPodAndBertResponse> BindPodAndBerth(BindPodAndBerthRquest param)
    {
      var content = new StringContent(JsonSerializer.Serialize(param).ToString(), Encoding.UTF8);
      var path = "rcs/services/rest/hikRpcService/bindPodAndBerth";
      var result = new BindPodAndBertResponse();

      try {
        var response = await _client.PostAsync(path, content);
        var text = await response.Content.ReadAsStringAsync();

        result = JsonSerializer.Deserialize<BindPodAndBertResponse>(text);

        return result;
      } catch (Exception e) {
        return new BindPodAndBertResponse {
          code = "3",
          message = $"操作执行失败，请重试: {e.Message}"
        };
      }
    }
  }

  public class BindPodAndBerthRquest
  {
    public string ReqCode { get; set; }

    public string PodCode { get; set; }

    public string PositionCode { get; set; }

    public string IndBind { get; set; }

    public BindPodAndBerthRquest(string podCode, string positionCode, string indBind, string reqCode = null)
    {
      ReqCode = reqCode ?? Guid.NewGuid().ToString();
      PodCode = podCode;
      PositionCode = positionCode;
      IndBind = indBind ?? "0";
    }
  }

  public class BindPodAndBertResponse
  {
    public string code { get; set; }

    public string data { get; set; }

    public string message { get; set; }

    public string reqCode { get; set; }

    public int rowCount { get; set; }

    public BindPodAndBertResponse()
    {

    }
  }
}
