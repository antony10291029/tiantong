using DBCore;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Namei.Aggregates
{
  public class AppSeederController: SeederControllerBase
  {
    private readonly AppContext _context;

    public AppSeederController(AppContext context, IMigrator migrator): base(migrator)
    {
      _context = context;
    }

    protected override void Seed()
    {
      InsertOrderItemReviewRecord();
    }

    private void InsertOrderItemReviewRecord()
    {
      var records = Enumerable.Range(1, 1000)
        .Select(i => OrderItemReviewRecord.From(
          type: "test",
          orderCode: $"test_order_id{i}",
          itemCode: $"test_item_id{i}"
        ));

      _context.AddRange(records);
      _context.SaveChanges();
    }
  }
}
