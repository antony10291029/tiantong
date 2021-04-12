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
      var asns = _wms.Set<WmsAsn>()
        .Where(asn => asn.FromName.Contains("总装车间"))
        .ToArray()
        .ToDictionary(asn => asn.CUSTOMER_BILL, asn => asn);

      var asnIds = asns.Values.Select(asn => asn.CUSTOMER_BILL);

      var ticketQuery = _wms.Set<WmsPickTicket>()
        .Where(ticket => asnIds.Contains(ticket.RelatedBill1));

      if (param.Query != "" && param.Query != null) {
        ticketQuery = ticketQuery.Where(ticket => ticket.Code.Contains(param.Query));
      }

      var tickets = ticketQuery
        .OrderByDescending(ticket => ticket.Id)
        .ToDataMap();

      var ticketIds = tickets.Entities.Values.Select(ticket => ticket.Id as long?);

      var moveDocs = _wms.Set<WmsMoveDoc>()
        .Where(doc => ticketIds.Contains(doc.RelatedBillId))
        .ToDataMap();

      var moveDocIds = moveDocs.Entities.Values.Select(doc => doc.Id as long?);

      var tasks = _wms.Set<WmsTask>()
        .Include(task => task.Item)
        .Include(task => task.ItemKey)
        .Include(task => task.Location)
        .Where(task => moveDocIds.Contains(task.MoveDocId))
        .OrderByDescending(task => task.CreatedAt)
          .ThenByDescending(task => task.Id)
        .Paginate(param);

      return new {
        page = tasks.Page,
        pageSize = tasks.PageSize,
        total = tasks.Total,
        keys = tasks.Keys,
        entities = tasks.Entities.ToDictionary(
          task => task.Key,
          data => {
            var task = data.Value;
            var moveDoc = task.MoveDocId is null ? null : moveDocs.Entities[(long) task.MoveDocId];
            var ticket = moveDoc is null ? null : moveDoc.RelatedBillId is null ? null : tickets.Entities[(long) moveDoc.RelatedBillId];
            var asn = ticket is null ? null : ticket.RelatedBill1 is null ? null : asns[ticket.RelatedBill1];

            return new {
              id = task.Id,
              createdAt = task.CreatedAt,
              orderNumber = ticket.Code,
              itemCode = task.Item.Code,
              itemName = task.Item.Name,
              barcode = task.MoveToolPalletNo,
              pickedQuantity = task.PickedQty,
              locationCode = task.Location.Code,
              fromName = asn.FromName
            };
          }
        )
      };
    }
  }
}
