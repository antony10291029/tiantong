using DBCore;

namespace Midos.Center.Database
{
  public class CreateConfigsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("000100_CreateConfigsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists configs");
    }
  }
}
