using System;
using Midos.Domain;

namespace Namei.Aggregates
{
  public static class OrderItemReviewType
  {
    public const string MoveDocBox = "move.doc.box";
  }

  public static class OrderItemReviewRecordStatus
  {
    public const string Created = "created";
  }

  public class OrderItemReviewRecord: IEntity, IAggregateRoot
  {
    public long Id { get; private set; }

    public string Type { get; private set; }

    public string OrderCode { get; private set; }

    public string ItemCode { get; private set; }

    public string Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private OrderItemReviewRecord() {}

    public static OrderItemReviewRecord From(
      string type,
      string orderCode,
      string itemCode
    ) => new OrderItemReviewRecord {
      Type = type,
      OrderCode = orderCode,
      ItemCode = itemCode,
      CreatedAt = DateTime.Now,
      Status = OrderItemReviewRecordStatus.Created,
    };

    public void Rebind()
    {
      CreatedAt = DateTime.Now;
    }
  }
}
