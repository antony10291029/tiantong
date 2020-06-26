using DBCore;

namespace Tiantong.Account.Database
{
  public class CreateEmailVerificationsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("000200_CreateEmailVerificationsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists email_verifications");
    }
  }
}
