using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Midos.Domain.Test.EventPublisher
{
  [TestClass]
  public class EventPublisherTest
  {
    [TestMethod]
    public void HasEvent_Success()
    {
      var publisher = new TestEventPublisher();
      
      publisher.Publish("test", new {});

      AssertHelper.HasEvent("test");
    }

    [TestMethod]
    public void HasEvent_Failed()
    {
      try {
        AssertHelper.HasEvent("test");
        Assert.Fail("HasEvent should fail when the event does not existed");
      } catch (Exception) {}
    }

    record TestEvent: DomainEvent
    {
      public string Id { get; init; }

      public string Name { get; init; }
    }

    [TestMethod]
    public void GetEvent()
    {
      var publisher = new TestEventPublisher();
      var @event = new TestEvent {
        Id = "01",
        Name = "foo"
      };

      publisher.Publish("test", @event);

      var res = AssertHelper.GetEvent<TestEvent>("test");

      Assert.AreEqual(@event, res);
    }

    [TestMethod]
    public void TestHasEvent()
    {
      var publisher = new TestEventPublisher();
      var @event = new TestEvent {
        Id = "01",
        Name = "foo"
      };

      publisher.Publish("test", @event);

      AssertHelper.HasEvent("test", @event);
    }
  }
}
