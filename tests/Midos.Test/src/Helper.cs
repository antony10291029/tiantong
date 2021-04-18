using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;

namespace Midos.Domain.Test
{
  public class Helper
  {
    public static TService UseService<TService>(Action<Mock<TService>> useMock = null)
      where TService: class
    {
      var mock = new Moq.Mock<TService>();

      if (useMock != null) {
        useMock(mock);
      }

      return mock.Object;
    }
  }

  public class AssertHelper
  {
    private static T[] GetEvents<T>(string name) where T: DomainEvent
      => TestEventPublisher.events[name]
        .Select(data => data as T)
        .ToArray();

    public static void HasEvent(string name)
    {
      if (!TestEventPublisher.events.ContainsKey(name)) {
        Assert.Fail($"event is not found: {name}");
      }
    }

    public static void HasEvent<T>(string name, T data) where T: DomainEvent
    {
      HasEvent(name);

      var events = GetEvents<T>(name);

      if (!events.Any(@event => @event == data)) {
        Assert.Fail($"event is not found: {name}, {data}");
      }
    }

    public static void HasNotEvent<T>(string name, T data) where T: DomainEvent
    {
      if (TestEventPublisher.events.ContainsKey(name)) {
        var events = GetEvents<T>(name);

        if (events.Any(@event => @event == data)) {
          Assert.Fail($"event should not be found: {name}, {data}");
        }
      }
    }

  }
}
