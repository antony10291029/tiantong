using Tiantong.Iot.Entities;
using Tiantong.Iot.DB.Sqlite;

namespace Tiantong.Iot.Api
{
  public class DatabaseFactory
  {
    public IotDbContext Resolve()
    {
      return new IotSqliteDbcontext();
    }

    public void Log<T>(T log)
    {
      using (var db = Resolve()) {
        db.Add(log);
        db.SaveChanges();
      }
    }

  }

}
