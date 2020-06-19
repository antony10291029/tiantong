using DBCore;

namespace Tiantong.Iot.Sqlite.Log
{
  public class CreateEmailVerifyCodeTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.001000_CreateEmailVerifyCodeTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists email_verify_code");
    }

  }

}
