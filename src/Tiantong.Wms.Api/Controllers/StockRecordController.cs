// using System.Linq;
// using System.ComponentModel.DataAnnotations;
// using Microsoft.AspNetCore.Mvc;
// using Renet.Web;

// namespace Tiantong.Wms.Api
// {
//   public class StockRecordController : BaseController
//   {
//     private IAuth _auth;

//     private WarehouseRepository _warehouses;

//     private StockRecordRepository _stockRecords;

//     public StockRecordController(
//       IAuth auth,
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
//       _auth.EnsureOwner();
//       var warehouseId = (int) param.warehouse_id;
//       _warehouses.EnsureOwner(warehouseId, _auth.User.id);

//       return _stockRecords.Table
//         .Where(record => record.warehouse_id == warehouseId)
//         .ToArray();
//     }
//   }
// }
