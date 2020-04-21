// using System.Linq;
// using System.ComponentModel.DataAnnotations;
// using Microsoft.AspNetCore.Mvc;
// using Renet.Web;

// namespace Tiantong.Wms.Api
// {
//   public class StockRecordController : BaseController
//   {
//     private Auth _auth;

//     private WarehouseRepository _warehouses;

//     private StockRecordRepository _stockRecords;

//     public StockRecordController(
//       Auth auth,
//       WarehouseRepository warehouses,
//       StockRecordRepository stockRecords
//     ) {
//       _auth = auth;
//       _warehouses = warehouses;
//       _stockRecords = stockRecords;
//     }

//     public class StockRecordSearchParams
//     {
//       [Required]
//       public int? warehouse_id { get; set; }
//     }

//     public StockRecord[] Search([FromBody] StockRecordSearchParams param)
//     {
//       _auth.EnsureUser();
//       var warehouseId = (int) param.warehouse_id;
//       _warehouses.EnsureUser(warehouseId, _auth.User.id);

//       return _stockRecords.Table
//         .Where(record => record.warehouse_id == warehouseId)
//         .ToArray();
//     }
//   }
// }
