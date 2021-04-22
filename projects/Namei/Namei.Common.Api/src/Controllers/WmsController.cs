using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Midos.Domain;
using Namei.Common.Entities;
using System.Linq;

namespace Namei.Common.Api
{
  public class WmsController: BaseController
  {
    private WmsContext _wms;

    private SapContext _sap;

    public WmsController(
      WmsContext wms,
      SapContext sap
    ) {
      _sap = sap;
      _wms = wms;
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
      public long Id { get; set; }
    }

    [HttpPost("/wms/pick-ticket-tasks/start")]
    public object Start([FromBody] StartParams param)
    {
      var task = _wms.Find<WmsTask>(param.Id);

      task.Start();
      _wms.SaveChanges();

      return NotifyResult
        .FromVoid()
        .Success("任务已下发");
    }

    [HttpPost("/wms/pick-ticket-tasks/finish")]
    public object Finish([FromBody] StartParams param)
    {
      var task = _wms.Find<WmsTask>(param.Id);

      task.Finish();
      _wms.SaveChanges();

      return NotifyResult
        .FromVoid()
        .Success("任务已完成");
    }

    [HttpPost("/wms/pick-ticket-tasks/close")]
    public object Close([FromBody] StartParams param)
    {
      var task = _wms.Find<WmsTask>(param.Id);

      task.Close();
      _wms.SaveChanges();

      return NotifyResult
        .FromVoid()
        .Success("任务已关闭");
    }

    [HttpPost("/wms/pick-ticket-tasks/reset")]
    public object Reset([FromBody] StartParams param)
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
