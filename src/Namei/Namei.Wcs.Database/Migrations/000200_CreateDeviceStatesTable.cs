using DBCore;

namespace Namei.Wcs.Database
{
  public class CreateDeviceStatesTable: IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("000200_CreateDeviceStatesTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists device_states");
    }
  }
}
