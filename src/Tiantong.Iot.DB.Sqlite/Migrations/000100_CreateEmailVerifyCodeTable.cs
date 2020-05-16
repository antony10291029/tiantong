using DBCore;

namespace Tiantong.Iot.DB.Sqlite
{
  public class CreateEmailVerifyCodeTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.000100_CreateEmailVerifyCodeTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists email_verify_code");
    }
  }
}
