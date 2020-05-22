using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tiantong.Iot.Test
{
  [TestFixture]
  public class IntervalTest
  {
    [Test]
    public void TestStart()
    {
      var interval = new Interval();

      interval.SetTime(1).SetHandler(() => interval.Stop());
      interval.Start();
      interval.WaitAsync().AssertFinishIn();
    }

    [Test]
    public void TestException()
    {
      var interval = new Interval(() => throw new Exception("ex"), 1);

      try {
        interval.Start().WaitAsync().AssertFinishIn();
        Assert.Fail("interval should throw the exception");
      } catch (Exception e) {
        Assert.AreEqual(e.Message, "ex");
      }
    }
  }
}
