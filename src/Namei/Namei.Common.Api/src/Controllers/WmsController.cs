using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Midos.Services.Http;
using Namei.Common.Entities;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Namei.Common.Api
{
  public class WmsController: BaseController
  {
    private readonly string _wcsUrl;

    private readonly WmsContext _wms;

    private readonly SapContext _sap;

    private readonly HttpClient _client;

    public WmsController(
      Config config,
      WmsContext wms,
      SapContext sap,
      IHttpClientFactory http
    ) {
      _sap = sap;
      _wms = wms;
      _client = http.CreateClient();

      if (config.IsProduction) {
        _wcsUrl = "http://172.16.2.64:5100";
      } else if (config.IsDevelopment) {
        _wcsUrl = "http://localhost:5100";
      } else {
        _wcsUrl = "http://172.16.2.74:5100";
      }
    }

    public class SearchAsnDetailBatch
    {
      public long AsnDetailId { get; set; }

      public bool WithoutWarehouse { get; set; }
    }

    [HttpPost("/wms/asn-batches/search")]
    public object SearchItemBatches([FromBody] SearchAsnDetailBatch param)
    {
      var asnDetail = _wms.AsnDetails
        .Include(ad => ad.Asn)
        .Include(ad => ad.Item)
        .FirstOrDefault(ad => ad.Id == param.AsnDetailId);

      if (asnDetail == null) {
        return Array.Empty<SapItemBatch>();
      }

      var fromName = asnDetail.Asn.FromName;
      var itemCode = asnDetail.Item.Code;

      var query = _sap.ItemBatches
        .Where(item => item.ItemCode == itemCode);

      if (!param.WithoutWarehouse) {
        query = query.Where(item => fromName.Contains(item.WarehouseCode));
      }

      return query.ToArray();
    }

    public record SearchPickTicketTaskParams
    {
      public DateTime Date { get; set; }
    }

    [HttpPost("/wms/pick-ticket-tasks/search")]
    public object SearchPickTicketTasks([FromBody] SearchPickTicketTaskParams param)
    {
      if (param.Date == default(DateTime)) {
        param.Date = DateTime.Now;
      }

      var min = param.Date.Add(-param.Date.TimeOfDay);
      var max = min.AddDays(1);

      Console.WriteLine(max);
      Console.WriteLine(min);

      var data =  _wms.Set<WmsPickTicketTask>()
        .Where(task => task.CreatedAt < max && task.CreatedAt > min)
        .OrderByDescending(task => task.OrderNumber)
        .ThenByDescending(task => task.ItemCode)
        .ThenByDescending(task => task.Id)
        .ToDataMap();

      return new {
        data.Keys,
        Values = data.Entities
      };
    }

    public class RestQuantityQueryParams
    {
      public string[] Codes { get; set; }
    }

    [HttpPost("/wms/inventory-rest-quantity/query")]
    public object QueryRestQuantity([FromBody] RestQuantityQueryParams param)
    {
      return _wms.Set<WmsInventoryRestQuantity>()
        .Where(quantity => param.Codes.Contains(quantity.PalletCode))
        .ToDictionary(quantity => quantity.PalletCode, quantity => quantity);
    }

    public record StartParams
    {
      public long TaskId { get; set; }

      public string Position { get; set; }

      public string Destination { get; set; }

      public string PalletCode { get; set; }
    }

    public record CreateTaskDTO
    {
      public string Message { get; set; }
    }

    [HttpPost("/wms/pick-ticket-tasks/start")]
    public async Task<object> Start([FromBody] StartParams param)
    {
      var task = _wms.Find<WmsPickTicketTask>(param.TaskId);
      var taskData = _wms.Find<WmsTask>(param.TaskId);

      if (param.Destination == null) {
        return NotifyResult
          .FromVoid()
          .Danger("任务终点不能为空");
      }

      if (param.Position == null) {
        param.Position = task.LocationCode;
      }

      if (param.PalletCode == null) {
        param.PalletCode = task.PalletCode;
      }

      var url = $"{_wcsUrl}/agc-tasks/create";
      var data = new {
        type = "warehouse.workshop",
        taskId = param.TaskId.ToString(),
        position = param.Position,
        destination = param.Destination,
        palletCode = param.PalletCode
      };

      var response = await _client.PostAsJsonAsync(url, data);
      var result = await response.Content.ReadFromJsonAsync<CreateTaskDTO>(
        new JsonSerializerOptions(JsonSerializerDefaults.Web)
      );

      if (((int) response.StatusCode) < 400) {
        taskData.Start();
        _wms.SaveChanges();

        return NotifyResult.FromVoid().Success("任务已下发");
      } else {
        return NotifyResult.FromVoid().Danger(result.Message);
      }
    }

    [HttpPost("/wms/pick-ticket-tasks/start-batch")]
    public object StartRange([FromBody] StartParams[] param)
    {
      var isSuccess = true;

      foreach (var item in param) {
        try {
          Start(item).GetAwaiter().GetResult();
        } catch {
          isSuccess = false;
        }
      }

      var notify =  NotifyResult.FromVoid();

      return isSuccess
        ? notify.Success("任务已全部下发")
        : notify.Danger("部分任务下发失败，请校验任务列表");
    }

    public class FinishParams
    {
      public string TaskId { get; set; }
    }

    [HttpPost("/wms/pick-ticket-tasks/finish")]
    public object Finish([FromBody] FinishParams param)
    {
      var id = long.Parse(param.TaskId);
      var task = _wms.Set<WmsTask>()
        .FirstOrDefault(task => task.Id == id);

      task.Finish();
      _wms.SaveChanges();

      return NotifyResult
        .FromVoid()
        .Success("任务已完成");
    }

    public struct CloseParams
    {
      public long Id { get; set; }
    }

    [HttpPost("/wms/pick-ticket-tasks/close")]
    public object Close([FromBody] CloseParams param)
    {
      var task = _wms.Find<WmsTask>(param.Id);

      task.Close();
      _wms.SaveChanges();

      return NotifyResult
        .FromVoid()
        .Success("任务已关闭");
    }

    [HttpPost("/wms/pick-ticket-tasks/reset")]
    public object Reset([FromBody] CloseParams param)
    {
      var task = _wms.Find<WmsTask>(param.Id);

      task.Reset();
      _wms.SaveChanges();

      return NotifyResult
        .FromVoid()
        .Success("任务已重制");
    }
  }
}
