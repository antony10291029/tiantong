using System;
using NUnit.Framework;

namespace Wcs.Plc.Test
{
  [TestFixture]
  public class IntervalTest
  {
    [Test]
    public void TestStart()
    {
      bool flag = false;
      var interval = new Interval();

      interval.SetTime(1).SetHandler(() => flag = true);
      interval.Start().WaitAsync().Wait(10);
      interval.Stop().Wait();

      Assert.IsTrue(flag);
    }

    [Test]
    public void TestException()
    {
      var interval = new Interval(() => throw new Exception("ex"), 1);

      try {
        interval.Start().WaitAsync().AssertFinishIn(10);
        Assert.Fail("interval should throw the exception");
      } catch (Exception) {
      }
    }
  }
}
