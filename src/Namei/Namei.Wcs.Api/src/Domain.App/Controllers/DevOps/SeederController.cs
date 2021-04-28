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

    private readonly DomainContext _domain;

    public SeederController(
      DomainContext domain,
      IMigrator migrator,
      IRandom random
    ): base(migrator) {
      _domain = domain;
      _random = random;
    }

    protected override void Seed()
    {
      SeedRcsAgcTasks();
      SeedRcsAgcTaskTypes();
    }

    private void SeedRcsAgcTaskTypes()
    {
      var types = Enumerable.Range(1, 100)
        .Select(i => RcsAgcTaskType.From(
          key: $"test_type_key_{i}",
          name: $"test_type_name_{i}",
          method: RcsAgcTaskMethod.Values.ToArray()[i % 7],
          webhook: "http://localhost:5100/"
        ));

      _domain.AddRange(types);
      _domain.SaveChanges();
    }

    private void SeedRcsAgcTasks()
    {
      foreach (var i in Enumerable.Range(1, 1000)) {
        var task = RcsAgcTask.From(
          taskType: "wcs.put",
          position: (1000 + i).ToString(),
          destination: (8000 + i).ToString(),
          podCode: (10000 + i).ToString(),
          comment: $"测试任务 {i}",
          orderType: "测试任务",
          orderId: _random.Int(100000, 999999)
        );

        if (_random.Int(1, 10) > 3) {
          task.Start(_random.Int(10000, 20000).ToString());
          if (_random.Int(1, 10) > 5) {
            task.Finish(_random.Int(1, 10).ToString());
          } else if (_random.Int(1, 10) > 3) {
            task.Close();
          }
        }

        _domain.Add(task);
      }

      _domain.SaveChanges();
    }
  }
}
