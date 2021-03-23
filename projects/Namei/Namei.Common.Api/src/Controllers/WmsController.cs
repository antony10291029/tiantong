using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
  }
}
