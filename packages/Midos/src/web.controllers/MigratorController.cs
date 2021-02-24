using DBCore;

namespace Microsoft.AspNetCore.Mvc
{
  public abstract class MigratorControllerBase: Controller
  {
    protected abstract IMigrator _migrator { get; set; }

    public class ResponseParams: MessageObject
    {
      public int Count { get; set; }

      public ResponseParams(int count)
      {
        Count = count;
      }
    }

    [HttpPost("/midos/migrations/migrate")]
    public object Migrate()
    {
      return NotifyResult
        .From(new ResponseParams(_migrator.Migrate()))
        .Success("数据库已迁移");
    }

    [HttpPost("/midos/migrations/rollback")]
    public INotifyResult<ResponseParams> Rollback()
    {
      return NotifyResult
        .From(new ResponseParams(_migrator.Rollback()))
        .Success("数据库已回滚");
    }

    [HttpPost("/midos/migrations/refresh")]
    public INotifyResult<ResponseParams> Refresh()
    {
      return NotifyResult
        .From(new ResponseParams(_migrator.Refresh()))
        .Success("数据库已刷新");
    }
  }
}
