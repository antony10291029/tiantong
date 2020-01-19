using System.Linq;
using System;
using System.Collections.Generic;
using Wcs.Plc.Database;
using Wcs.Plc.Entities;

namespace Wcs.Plc
{
  public class EventLogger : IEventPlugin
  {
    private DbContext _db;

    private List<EventLog> _eventLogs;

    public IInterval Interval;

    public EventLogger(IPlcContainer container)
    {
      Interval = new Interval();
      _eventLogs = new List<EventLog>();
      _db = container.ResolveDbContext();

      Interval.SetTime(500);
      Interval.SetHandler(HandleEventLogs);
      container.IntervalManager.Add(Interval);
    }

    public void HandleEventLogs()
    {
      EventLog[] logs;

      lock (_eventLogs) {
        logs = _eventLogs.ToArray();
        _eventLogs.Clear();
      }

      _db.EventLogs.AddRange(logs);
      _db.SaveChanges();
    }

    public void Install(IEvent event_)
    {
      event_.All(args => {
        var log = new EventLog {
          Key = args.Key,
          Payload = args.Payload,
          HandlerCount = args.HandlerCount,
        };

        lock (_eventLogs) {
          _eventLogs.Add(log);
        }
      });
    }
  }
}
