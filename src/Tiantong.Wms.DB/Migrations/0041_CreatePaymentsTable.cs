
using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreatePaymentsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0041_CreatePaymentsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists payments");
    }
  }
}
