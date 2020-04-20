using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateEmailVerificationsTable : IMigration
  {
    public void Up(DBCore.DbContext db)
    {
      db.ExecuteFromSql("Migration.001100_CreateEmailVerificationsTable");
    }

    public void Down(DBCore.DbContext db)
    {
      db.ExecuteSql("drop table if exists email_verifications");
    }
  }
}
