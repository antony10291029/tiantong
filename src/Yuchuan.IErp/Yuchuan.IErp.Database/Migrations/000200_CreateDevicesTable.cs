using DBCore;

namespace Yuchuan.IErp.Database
{
  public class CreateDevicesTable: IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("000200_CreateDevicesTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists device_states");
      db.ExecuteSql("drop table if exists devices");
    }
  }
}
