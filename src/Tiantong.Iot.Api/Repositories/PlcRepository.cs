using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tiantong.Iot.Entities;
using Renet.Web;

namespace Tiantong.Iot.Api
{
  public class PlcRepository
  {
    public IotDbContext _db;

    public PlcRepository(IotDbContext db)
    {
      _db = db;
    }

    public void Add(Plc plc)
    {
      plc.id = 0;

      _db.Add(plc);
      _db.SaveChanges();
    }

    public void Delete(int id)
    {
      var plc = EnsureGet(id);
      _db.Plcs.Remove(plc);
      _db.SaveChanges();
    }

    public void Update(Plc plc)
    {
      var oldPlc = EnsureGet(plc.id);
      _db.Entry(oldPlc).CurrentValues.SetValues(plc);
      _db.Entry(oldPlc).Property(p => p.created_at).IsModified = false;
      _db.SaveChanges();
    }

    public Plc[] All()
    {
      return _db.Plcs.ToArray();
    }

    public Plc Get(int id)
    {
      return _db.Plcs.FirstOrDefault(p => p.id == id);
    }

    public Plc EnsureGet(int id)
    {
      var plc = Get(id);

      if (plc == null) {
        throw new FailureOperation("plc 不存在");
      }

      return plc;
    }
  }
}
