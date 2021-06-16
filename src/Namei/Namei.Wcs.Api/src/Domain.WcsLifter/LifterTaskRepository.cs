using System;
using System.Linq;
using Namei.Wcs.Api;

namespace Namei.Wcs.Aggregates
{
  public interface ILifterTaskRepository
  {
    LifterTask Add(
      string lifterId, string floor, string barcode,
      string destination, string data, string from
    );

    void SaveChanges();

    LifterTask FindFromRuntimeBarcode(string barcode);

    LifterTask[] FindRangeFromExported(string lifterId, string floor);
  }

  public class LifterTaskRepository: ILifterTaskRepository
  {
    private readonly WcsContext _context;

    public LifterTaskRepository(WcsContext context)
      => _context = context;

    public LifterTask Add(
      string lifterId, string floor, string barcode,
      string destination, string data, string from
    ) {
      var task = new LifterTask(lifterId, floor, barcode, destination, data, from);

      _context.Add(task);

      return task;
    }

    public void SaveChanges() => _context.SaveChanges();

    public LifterTask FindFromRuntimeBarcode(string barcode)
    {
      var time = DateTime.Now.AddHours(-10);

      return _context.Set<LifterTask>()
        .Where(task => task.ImportedAt > time)
        .Where(task => task.Barcode == barcode)
        .OrderByDescending(task => task.Id)
        .FirstOrDefault(task =>
          task.Status == LifterTaskStatus.Imported ||
          task.Status == LifterTaskStatus.Exported 
        );
    }

    public LifterTask[] FindRangeFromExported(string lifterId, string floor)
    {
      return _context.Set<LifterTask>()
        .Where(task => task.LifterId == lifterId)
        .Where(task => task.Destination == floor)
        .Where(task => task.Status == LifterTaskStatus.Exported)
        .ToArray();
    }
  }
}
