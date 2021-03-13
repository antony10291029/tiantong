using DotNetCore.CAP;
using System;
using System.Net.Mime;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Namei.Wcs.Api
{
  public class PalletInfo
  {
    public string Destination { get; set; } = "2";

    public string TaskId { get; set; } = "0001";

    public string Barcode { get; set; }
  }

  public class WmsService
  {
    public static string Destination = "1";

    private ICapPublisher _cap;

    private HttpClient _client;

    public WmsService(ICapPublisher cap, IHttpClientFactory factory)
    {
      _cap = cap;
      _client = factory.CreateClient();
      _client.Timeout = new TimeSpan(0, 0, 10);
      _client.BaseAddress = new System.Uri("http://172.16.2.52:8086");
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
      var taskid = "";
      var destination =  "";

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
