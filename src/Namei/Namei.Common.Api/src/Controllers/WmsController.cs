using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Midos.Domain;
using Midos.Services.Http;
using Namei.Common.Entities;
using System.Linq;

namespace Namei.Common.Api
{
  public class WmsController: BaseController
  {
    private string _wcsUrl;

    private WmsContext _wms;

    private SapContext _sap;

    private IHttpService _http;

    public WmsController(
      Config config,
      WmsContext wms,
      SapContext sap,
      IHttpService http
    ) {
      _sap = sap;
      _wms = wms;
      _http = http;
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
        return new SapItemBatch[] {};
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

    [HttpPost("/wms/pick-ticket-tasks/search")]
    public object SearchPickTicketTasks([FromBody] QueryParams param)
    {
      return _wms.Set<WmsPickTicketTask>()
        .OrderByDescending(task => task.Id)
        .ThenByDescending(task => task.FromName)
        .ThenByDescending(task => task.RestQuantity)
        .Paginate(param);
    }

    public struct StartParams
    {
      public long TaskId { get; set; }

      public string Position { get; set; }

      public string Destination { get; set; }

      public string PalletCode { get; set; }
    }

    [HttpPost("/wms/pick-ticket-tasks/start")]
    public object Start([FromBody] StartParams param)
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

      _http.Post<object, object>(
        url: $"{_wcsUrl}/wms/pick-ticket-tasks/start",
        data: param
      );

      taskData.Start();
      _wms.SaveChanges();

      return NotifyResult
        .FromVoid()
        .Success("任务已下发");
    }

    public class FinishParams
    {
      public long Id { get; set; }
    }

    [HttpPost("/wms/pick-ticket-tasks/finish")]
    public object Finish([FromBody] FinishParams param)
    {
      var task = _wms.Find<WmsTask>(param.Id);

      task.Finish();
      _wms.SaveChanges();

      return NotifyResult
        .FromVoid()
        .Success("任务已完成");
    }

    [HttpPost("/wms/pick-ticket-tasks/close")]
    public object Close([FromBody] FinishParams param)
    {
      var task = _wms.Find<WmsTask>(param.Id);

      task.Close();
      _wms.SaveChanges();

      return NotifyResult
        .FromVoid()
        .Success("任务已关闭");
    }

    [HttpPost("/wms/pick-ticket-tasks/reset")]
    public object Reset([FromBody] FinishParams param)
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
