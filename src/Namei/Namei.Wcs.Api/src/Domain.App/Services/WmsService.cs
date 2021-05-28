using DotNetCore.CAP;
using System;
using System.Net.Mime;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Namei.Wcs.Api
{
  public interface IWmsService
  {
    PalletInfo GetPalletInfo(string barcode);

    string RequestPicking(string lifterId, string floor, string barcode, string taskId);
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
      _client.BaseAddress = new Uri(config.WmsUrl);
      _client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json)
      );
    }

    public PalletInfo GetPalletInfo(string barcode)
    {
      var json = JsonSerializer.Serialize(new {
        method = "ReqTask",
        BARCODE = barcode,
      });
      var content = new StringContent(json, Encoding.UTF8);
      var response = _client.PostAsync("/namei_wms/wcsCallback/request", content).GetAwaiter().GetResult();
      var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
      string taskid;
      string destination;

      try {
        var dom = JsonDocument.Parse(result);
        destination = dom.RootElement.GetProperty("TO_LF").GetString();
        taskid = dom.RootElement.GetProperty("taskid").GetString();
      } catch {
        throw new Exception("任务查询失败：" + result);
      }

      if (taskid == "") {
        throw new Exception("任务查询失败：" + result);
      }

      return new PalletInfo() {
        Barcode = barcode,
        Destination = destination,
        TaskId = taskid,
      };
    }

    public string RequestPicking(string lifterId, string floor, string barcode, string taskId)
    {
      var json = JsonSerializer.Serialize(new {
        method = "FinishReport",
        LF_NO = $"LF0{lifterId}",
        TO_LF = $"LF0{floor}",
        taskid = taskId,
        BARCODE = barcode,
      });

      var content = new StringContent(json, Encoding.UTF8);
      var response = _client.PostAsync("/namei_wms/wcsCallback/request", content).GetAwaiter().GetResult();
      var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

      return result;
    }
  }
}
