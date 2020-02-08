using NUnit.Framework;

namespace Wcs.Plc.Test
{
  [TestFixture]
  public class IntervalTest
  {
    [Test]
    public void TestTryWait()
    {
      var interval = new Interval();

      try {
        interval.SetHandler(() => {});
        interval.Start().TryWait(1);
        Assert.Fail("expect to throw exception when Interval.TryWait is timeout");
      } catch {}
    }

    [Test]
    public void TestStart()
    {
      bool flag = false;
      var interval = new Interval();

      interval.SetTimes(1).SetHandler(() => flag = true);
      interval.Start().TryWait();

      Assert.IsTrue(flag);
    }

    [Test]
    public void TestWithTimesZero()
    {
      var interval = new Interval();

      try {
        interval.SetTimes(0).SetHandler(() => {});
        interval.Start().TryWait(5);
        Assert.Fail("expect not to stop interval with Interval.SetTime(0)");
      } catch {}
    }

    [TestCase(1)]
    [TestCase(10)]
    public void TestWithTimes(int n)
    {
      var times = 0;
      var interval = new Interval();

      interval.SetTime(0).SetTimes(n).SetHandler(() => times++);
      interval.Start().TryWait();

      Assert.AreEqual(n, times);
    }

    [Test]
    public void TestWithoutTimes()
    {
      var times = 0;
      var interval = new Interval();

      interval.SetHandler(() => times++);
      interval.Start().Stop();

      Assert.True(true);
    }

    [TestCase(0, 1)]
    [TestCase(1, 1)]
    [TestCase(10, 10)]
    public void TestMultipleRun(int n, int m)
    {
      var times = 0;
      var interval = new Interval();

      interval.SetTimes(m).SetHandler(() => times++);
      for (var i = 0; i < n; i++) {
        interval.Start().TryWait();
      }

      Assert.AreEqual(times, n * m);
    }
  }
}
