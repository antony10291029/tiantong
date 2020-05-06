using Tiantong.Iot.Entities;
using DBCore;

namespace Tiantong.Iot.Api
{
  public class MigratorProvider
  {
    private IotDbContext _db;

    private IMigrator _mg;

    public MigratorProvider(IotDbContext db, IMigrator mg)
    {
      _db = db;
      _mg = mg;
    }
  }
}
