using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateEmailVerificationsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.001100_CreateEmailVerificationsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists email_verifications");
    }
  }
}
