using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Namei.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Namei.Common.Api
{
  public class AppController: BaseController
  {
    private Config _config;

    private WmsContext _wms;

    public AppController(Config config, WmsContext wms)
    {
      _config = config;
      _wms = wms;
    }

    [Route("/")]
    public object Home()
    {
      // var tasks = _wms.Set<WmsTask>()
      //   .Include(task => task.MoveDoc)
      //     .ThenInclude(moveDoc => moveDoc.PickTicket)
      //       .ThenInclude(ticket => ticket.Asn)
        // .Include(task => task.Location)
        // .Include(task => task.Item)
        // .Where(task => task.MoveDoc.PickTicket.Asn.FromName.Contains("总装车间"))
        // .Where(task => task.MoveDoc.PickTicket.Asn.BillTypeId == 15)
        // .AsNoTracking()
        // .OrderByDescending(task => task.CreatedAt)
        // .Take(10)
        // .PaginateNext(1, 15);

      // var inventories = _wms.Set<WmsInventory>()
      //   .Include(inventory => inventory.Item)
      //     .ThenInclude(WmsItemKey => WmsItemKey.)
      //   .Take(100)
      //   .ToDataMap();
      var tasks = _wms.Set<WmsAsn>()
        .Include(asn => asn.PickTickets)
        .Where(asn => asn.FromName.Contains("总装车间"))
        .Where(asn => asn.BillTypeId == 15)
        .PaginateNext(1, 15);

      var ids = tasks.Entities
        .SelectMany(entity => entity.Value.PickTickets.Select(ticket => ticket.Id))
        .ToArray();

      var moveDocs = _wms.Set<WmsMoveDoc>()
        .Include(moveDoc => moveDoc.Tasks)
        .Where(moveDoc => ids.Contains(moveDoc.RelatedBillId))
        .ToDataMap();

      var palletCodes = moveDocs.Entities
        .SelectMany(moveDoc => moveDoc.Value.Tasks.Select(task => task.MoveToolPalletNo));

      var inventories = _wms.Set<WmsInventory>()
        .Include(inventory => inventory.ItemKey)
        .Where(inventory => palletCodes.Contains(inventory.MoveToolPalletNo))
        .Where(inventory => inventory.QtyBaseQty != 0)
        .ToDataMap();

      return inventories;
    }
  }
}
