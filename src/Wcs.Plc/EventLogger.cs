using System.Linq;
using System;
using System.Collections.Generic;
using Wcs.Plc.Database;
using Wcs.Plc.Entities;

namespace Wcs.Plc
{
  public class EventLogger : EventPlugin
  {
    private DbContext _db;

    public Interval Interval = new Interval();

    private List<EventLog> _eventLogs = new List<EventLog>();

    public EventLogger(IntervalManager manager, DbContext database)
    {
      _db = database;
      manager.Add(Interval);
      Interval.SetTime(500);
      Interval.SetHandler(HandleEventLogs);
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

    public override void Install(Event event_)
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
