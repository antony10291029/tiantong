using System.Linq;
using DBCore;
using Microsoft.AspNetCore.Mvc;
using Midos.Utils;
using Namei.Wcs.Aggregates;

namespace Namei.Wcs.Api
{
  public class SeederController: SeederControllerBase
  {
    private readonly IRandom _random;

    private readonly WcsContext _context;

    public SeederController(
      WcsContext context,
      IMigrator migrator,
      IRandom random
    ): base(migrator) {
      _context = context;
      _random = random;
    }

    protected override void Seed()
    {
      InsertAgcTaskTypes();
      InsertAgcTasks();
    }

    private void InsertAgcTaskTypes()
    {
      var types = Enumerable.Range(1, 100)
        .Select(i => AgcTaskType.From(
          key: $"test_type_key_{i}",
          name: $"test_type_name_{i}",
          method: AgcTaskMethod.Values.ToArray()[i % 7],
          webhook: "http://localhost:5100/"
        ));

      _context.AddRange(types);
      _context.SaveChanges();
    }

    private void InsertAgcTasks()
    {
      var types = _context.Set<AgcTaskType>().ToArray();

      foreach (var i in Enumerable.Range(1, 1000)) {
        var task = AgcTask.From(
          typeId: _random.Array(types).Id,
          position: (1000 + i).ToString(),
          destination: (8000 + i).ToString(),
          podCode: (10000 + i).ToString(),
          taskId: _random.Int(100000, 999999).ToString(),
          priority: "5",
          agcCode: _random.Int(1, 5).ToString()
        );

        if (_random.Int(1, 10) > 3) {
          task.Start(_random.Int(10000, 20000).ToString());
          if (_random.Int(1, 10) > 5) {
            task.Finish(_random.Int(1, 10).ToString());
          } else if (_random.Int(1, 10) > 3) {
            task.Close();
          }
        }

        _context.Add(task);
      }

      _context.SaveChanges();
    }
  }
}
