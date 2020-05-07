using System.Linq;
using Tiantong.Iot.Entities;
using Renet.Web;

namespace Tiantong.Iot.Api
{
  public class PlcStateRepository
  {
    private IotDbContext _db;

    public PlcStateRepository(IotDbContext db)
    {
      _db = db;
    }

    public void Add(PlcState state)
    {
      _db.PlcStates.Add(state);
      _db.SaveChanges();
    }

    public void Delete(int id)
    {
      var state = _db.PlcStates.FirstOrDefault(s => s.id == id);

      if (state == null) {
        throw new FailureOperation("PLC 数据点不存在");
      }

      _db.PlcStates.Remove(state);
      _db.SaveChanges();
    }

    public void Update(PlcState state)
    {
      var oldState = _db.PlcStates.FirstOrDefault(s => s.id == state.id);

      if (oldState == null) {
        throw new FailureOperation("PlC 数据点不存在");
      }

      _db.Entry(oldState).CurrentValues.SetValues(state);
      _db.Entry(oldState).Property(oldstate => oldstate.plc_id).IsModified = false;
      _db.SaveChanges();
    }

    public PlcState[] All(int plcId)
    {
      return _db.PlcStates.Where(s => s.plc_id == plcId).ToArray();
    }

  }
}
