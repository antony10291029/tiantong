using DBCore;

namespace Namei.Wcs.Database
{
  public class CreateWcsDoorPassportsTable: IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("000600_CreateWcsDoorPassportsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists \"WcsDoorPassports\"");
    }
  }
}
