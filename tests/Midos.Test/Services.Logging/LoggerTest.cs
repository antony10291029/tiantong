using DotNetCore.CAP.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Midos.Services.Logging.Test
{
  [TestClass]
  public class LoggerTest
  {
    [TestMethod]
    public void Test_Log()
    {
      var category = "test.category";
      var logLevel = LogLevel.Information;
      var eventId = new EventId(1);
      var exception = new Exception("test");
      var state = new List<KeyValuePair<string, object>>() {
        new("foo", 1),
        new("bar", "bar"),
      };
      var logger = new MidosLogger(category);

      logger.Log(
        logLevel: logLevel,
        eventId: eventId,
        exception: exception,
        formatter: null,
        state: state
      );

      var logs = logger.Get();
      var log = logs.First();

      Assert.AreEqual(logs.Length, 1);
      Assert.AreEqual(log.Category, category);
      Assert.AreEqual(log.Level, LogLevel.Information.ToString());
      Assert.AreEqual(log.EventId, eventId.ToString());
      Assert.AreEqual(
        JsonSerializer.Serialize(log.Data),
        JsonSerializer.Serialize(
          new {
            Keys = state.Select(kv => kv.Key),
            Values = state.ToDictionary(kv => kv.Key, kv => kv.Value)
          }
        )
      );
    }
  }
}
