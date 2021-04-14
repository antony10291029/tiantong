using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Midos.Domain.Test
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
  }
}
