// using System.Linq;
// using System.ComponentModel.DataAnnotations;
// using Microsoft.AspNetCore.Mvc;
// using Renet.Web;

// namespace Tiantong.Wms.Api
// {
//   public class StockController : BaseController
//   {
//     private Auth _auth;

//     private ItemRepository _items;

//     private StockRepository _stocks;

//     private WarehouseRepository _warehouses;

//     public StockController(
//       Auth auth,
//       ItemRepository items,
//       StockRepository stocks,
//       WarehouseRepository warehouses
//     ) {
//       _auth = auth;
//       _items = items;
//       _stocks = stocks;
//       _warehouses = warehouses;
//     }

//     public class SearchParams
//     {
//       [Nonzero]
//       public int warehouse_id { get; set; }

//       [Nonzero]
//       public int page { get; set; }

//       [Nonzero]
//       public int page_size { get; set; }

//       public string search { get; set; }
//     }

//     public IPagination<Stock> Search([FromBody] SearchParams param)
//     {
//       _auth.EnsureUser();
//       _warehouses.EnsureUser(param.warehouse_id, _auth.User.id);

//       return _stocks.Table
//         .Where(stock => stock.warehouse_id == param.warehouse_id)
//         .Paginate(param.page, param.page_size);
//     }

//   }
// }
