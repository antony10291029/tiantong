using Renet;
using System.Collections.Generic;

namespace Namei.Wcs.Api
{
  public abstract class DoorServiceManager
  {
    private Dictionary<int, DoorService> _doors = new Dictionary<int, DoorService>();

    public DoorService Get(int id)
    {
      if (!_doors.ContainsKey(id)) {
        throw KnownException.Error($"设备 `自动门{id}`不存在");
      }

      return _doors[id];
    }
  }

  public abstract class DoorService
  {
    public abstract void Open();

    public abstract void Close();

    public abstract bool IsAvaliable();
  }
}
