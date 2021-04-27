// using Microsoft.AspNetCore.Mvc;
// using Midos.Domain;
// using System.Linq;

// namespace Namei.Aggregates
// {
//   public class OrderItemReviewRecordController: BaseController
//   {
//     private readonly AppContext _context;

//     public OrderItemReviewRecordController(AppContext context)
//     {
//       _context = context;
//     }

//     public class BindParams
//     {
//       public string Type { get; set; }

//       public string OrderCode { get; set; }

//       public string ItemCode { get; set; }
//     }

//     [HttpPost("/app/order-item-review-records/bind")]
//     public INotifyResult<IMessageObject> Bind([FromBody] BindParams param)
//     {
//       var record = _context.Set<OrderItemReviewRecord>()
//         .FirstOrDefault(record =>
//           record.Type == param.Type &&
//           record.OrderCode == param.OrderCode &&
//           record.ItemCode == param.ItemCode
//         );

//       if (record == null) {
//         _context.Add(OrderItemReviewRecord.From(
//           type: param.Type,
//           orderCode: param.OrderCode,
//           itemCode: param.ItemCode
//         ));
//       } else {
//         record.Rebind();
//       }

//       _context.SaveChanges();

//       return NotifyResult
//         .FromVoid()
//         .Success("订单明细绑定成功");
//     }

//     public class RemoveParams
//     {
//       public long Id { get; set; }
//     }

//     [HttpPost("/app/order-item-review-records/delete")]
//     public INotifyResult<IMessageObject> Delete([FromBody] RemoveParams param)
//     {
//       var result = NotifyResult.FromVoid();
//       var record = _context.Find<OrderItemReviewRecord>(param.Id);

//       if (record == null) {
//         return result.Success("订单明细不存在");
//       }

//       _context.Remove(record);
//       _context.SaveChanges();

//       return result.Success("订单明细已删除");
//     }

//     public class PaginateParams: QueryParams
//     {
//       public string Type { get; set; }
//     }

//     [HttpPost("/app/order-item-review-records/search")]
//     public IPagination<OrderItemReviewRecord> Paginate([FromBody] PaginateParams param)
//     {
//       var query = _context.Set<OrderItemReviewRecord>().AsQueryable();

//       if (param.Type != null && param.Type != "") {
//         query = query.Where(record => record.Type == param.Type);
//       }

//       if (param.Query != null && param.Query != "") {
//         query = query.Where(record =>
//           record.ItemCode.Contains(param.Query) ||
//           record.OrderCode.Contains(param.Query)
//         );
//       }

//       return query
//         .OrderByDescending(record => record.Id)
//         .Paginate(param);
//     }
//   }
// }
