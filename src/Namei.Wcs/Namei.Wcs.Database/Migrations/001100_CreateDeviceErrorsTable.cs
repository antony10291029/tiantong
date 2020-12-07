using DBCore;

namespace Namei.Wcs.Database
{
  public class CreateDeviceErrorsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("001100_CreateDeviceErrorsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists device_errors");
    }
  }
}
