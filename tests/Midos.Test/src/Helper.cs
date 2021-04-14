using System;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    public static void HasEvent(string name)
    {
      if (!TestEventPublisher.events.ContainsKey(name)) {
        Assert.Fail($"event is not found: {name}");
      }
    }
  }
}
