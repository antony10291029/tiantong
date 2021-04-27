using Microsoft.AspNetCore.Mvc;
using Midos.Domain;
using System.Linq;

namespace Namei.Aggregates
{
  public class MoveDocBoxController: BaseController
  {
    private readonly AppContext _context;

    public MoveDocBoxController(AppContext context)
    {
      _context = context;
    }

    public class BindParams
    {
      public string OrderCode { get; set; }

      public string ItemCode { get; set; }
    }

    [HttpPost("/wms/move-doc-boxes/bind")]
    public INotifyResult<IMessageObject> Bind([FromBody] BindParams param)
    {
      var record = _context.Set<OrderItemReviewRecord>()
        .FirstOrDefault(record =>
          record.Type == OrderItemReviewType.MoveDocBox &&
          record.OrderCode == param.OrderCode &&
          record.ItemCode == param.ItemCode
        );

      if (record == null) {
        _context.Add(OrderItemReviewRecord.From(
          type: OrderItemReviewType.MoveDocBox,
          orderCode: param.OrderCode,
          itemCode: param.ItemCode
        ));
      } else {
        record.Rebind();
      }

      _context.SaveChanges();

      return NotifyResult
        .FromVoid()
        .Success("作业单箱码绑定成功");
    }

    public class RemoveParams
    {
      public long Id { get; set; }
    }

    [HttpPost("/wms/move-doc-boxes/delete")]
    public INotifyResult<IMessageObject> Delete([FromBody] RemoveParams param)
    {
      var result = NotifyResult.FromVoid();
      var record = _context.Find<OrderItemReviewRecord>(param.Id);

      if (record == null) {
        return result.Success("订单明细不存在");
      }

      _context.Remove(record);
      _context.SaveChanges();

      return result.Success("订单明细已删除");
    }

    [HttpPost("/wms/move-doc-boxes/search")]
    public IPagination<OrderItemReviewRecord> Paginate([FromBody] QueryParams param)
    {
      var query = _context.Set<OrderItemReviewRecord>()
        .Where(record => record.Type == OrderItemReviewType.MoveDocBox);

      if (param.Query != null && param.Query != "") {
        query = query.Where(record =>
          record.ItemCode.Contains(param.Query) ||
          record.OrderCode.Contains(param.Query)
        );
      }

      return query
        .OrderByDescending(record => record.Id)
        .Paginate(param);
    }
  }
}
