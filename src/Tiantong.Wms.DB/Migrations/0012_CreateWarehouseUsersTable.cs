using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateWarehouseUsersTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0012_CreateWarehouseUsersTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists warehouse_users");
    }
  }
}
