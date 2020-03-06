using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class StockController : BaseController
  {
    private IAuth _auth;

    private WarehouseRepository _warehouses;

    private StockRepository _stocks;

    public StockController(
      IAuth auth,
      StockRepository stocks,
      WarehouseRepository warehouses
    ) {
      _auth = auth;
      _stocks = stocks;
      _warehouses = warehouses;
    }

    public class OrderCategorySearchParams
    {
      [Required]
      public int? warehouse_id { get; set; }
    }

    public Stock[] Search([FromBody] OrderCategorySearchParams param)
    {
      _auth.EnsureOwner();
      var warehouseId = (int) param.warehouse_id;
      _warehouses.EnsureOwner(warehouseId, _auth.User.id);

      return _stocks.Search(warehouseId);
    }
  }
}
