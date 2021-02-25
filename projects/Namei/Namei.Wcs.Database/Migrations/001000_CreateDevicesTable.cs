using DBCore;

namespace Namei.Wcs.Database
{
  public class CreateDevicesTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("001000_CreateDevicesTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists devices");
    }
  }
}
