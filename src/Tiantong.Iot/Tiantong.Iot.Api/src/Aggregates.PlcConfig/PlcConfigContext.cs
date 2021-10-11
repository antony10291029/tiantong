using System;
using System.Linq;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  public class PlcConfigContext
  {
    private readonly AppContext _context;

    public PlcConfigContext(AppContext context)
    {
      _context = context;
    }

    public Plc Get(int id)
    {
      var plc =  _context.Set<Plc>().FirstOrDefault(plc => plc.id == id);

      if (plc is null) {
        throw KnownException.Error("PLC 不存在", 404);
      }

      return plc;
    }

    public void Update(Plc plc)
    {
      if (_context.Set<Plc>().Any(p => p.name == plc.name && p.id != plc.id)) {
        throw KnownException.Error("设备名称已存在");
      }

      var oldPlc = Get(plc.id);
      _context.Entry(oldPlc).CurrentValues.SetValues(plc);
      _context.Entry(oldPlc).Property(p => p.created_at).IsModified = false;
      _context.SaveChanges();
    }
  }
}
