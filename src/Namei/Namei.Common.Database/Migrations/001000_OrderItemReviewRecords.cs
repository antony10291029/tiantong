using DBCore;

namespace Namei.Common.Database
{
  public class OrderItemReviewRecords: IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("001000_OrderItemReviewRecords");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists \"OrderItemReviewRecords\"");
    }
  }
}
