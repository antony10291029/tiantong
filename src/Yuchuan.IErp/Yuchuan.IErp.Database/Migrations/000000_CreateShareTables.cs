using DBCore;

namespace Yuchuan.IErp.Database
{
  public class CreateShareTables : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("000000_CreateShareTables");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists verify_emails");
    }
  }
}
