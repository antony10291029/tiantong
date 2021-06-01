using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Namei.Wcs.Api
{
  public interface IWmsService
  {
    Task<PalletInfo> GetPalletInfo(string barcode);

    Task<string> RequestPicking(string lifterId, string floor, string barcode, string taskId);
  }

  public class PalletInfo
  {
    public string Destination { get; set; } = "2";

    public string TaskId { get; set; } = "0001";

    public string Barcode { get; set; }
  }

  public class WmsService: IWmsService
  {
    public readonly static string Destination = "1";

    private readonly HttpClient _client;

    public WmsService(IHttpClientFactory factory, Config config)
    {
      _client = factory.CreateClient();
      _client.Timeout = new TimeSpan(0, 0, 10);
      _client.BaseAddress = new Uri(config.ApiGatewayUrl);
    }

    public async Task<PalletInfo> GetPalletInfo(string barcode)
    {
      var param = new {
        method = "ReqTask",
        BARCODE = barcode,
      };
      var url = "/wms/wcsCallback/request";
      string taskid;
      string destination;
      string result = "";
      HttpResponseMessage response;

      try {
        response = await _client.PostAsJsonAsync(url, param);
      } catch (Exception e) {
        throw new Exception("任务查询失败：" + e.Message);
      }

      try {
        result = await response.Content.ReadAsStringAsync();

        var dom = JsonDocument.Parse(result);

        destination = dom.RootElement.GetProperty("TO_LF").GetString();
        taskid = dom.RootElement.GetProperty("taskid").GetString();
      } catch {
        throw new Exception("任务查询失败：" + result);
      }

      return new PalletInfo() {
        Barcode = barcode,
        Destination = destination,
        TaskId = taskid,
      };
    }

    public async Task<string> RequestPicking(string lifterId, string floor, string barcode, string taskId)
    {
      var param = new {
        method = "FinishReport",
        LF_NO = $"LF0{lifterId}",
        TO_LF = $"LF0{floor}",
        taskid = taskId,
        BARCODE = barcode,
      };
      var url = "/wms/wcsCallback/request";
      var response = await _client.PostAsJsonAsync(url, param);
      var result = await response.Content.ReadAsStringAsync();

      return result;
    }
  }
}
