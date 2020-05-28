using System.Threading.Tasks;
using System;
using System.Threading;
using NUnit.Framework;

namespace Tiantong.Iot.Test
{
  [TestFixture]
  public class IntervalManagerTest
  {
    [Test]
    public void TestStop()
    {
      var manager = new IntervalManager();

      manager.Start().Stop().WaitAsync().AssertFinishIn(1000);
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(5)]
    public void TestRemove(int n)
    {
      var intervals = new Interval[n];
      var manager = new IntervalManager();

      for (var i = 0; i < n; i++) {
        intervals[i] = new Interval();
        intervals[i].SetHandler(() => {}).SetTime(0);
        manager.Add(intervals[i]);
      }

      manager.Start();
      for (var i = 0; i < n; i++) {
        var interval = intervals[i];
        manager.Remove(interval);
      }

      manager.Stop().Wait();

      Assert.AreEqual(1, manager.Intervals.Count);
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(10)]
    public void TestRunClear(int n)
    {
      var times = 0;
      var manager = new IntervalManager();

      for (var i = 0; i < n; i++) {
        var interval = new Interval();
        interval.SetHandler(() => times++).SetTime(0);
        manager.Add(interval);
      }

      manager.Start();
      manager.Clear();
      manager.WaitAsync().AssertFinishIn();

      Assert.IsTrue(true);
    }

    [Test]
    public void TestException()
    {
      var manager = new IntervalManager();

      manager.Add(new Interval(() => throw new Exception("ex"), 0));

      try {
        manager.Start();
        manager.WaitAsync().AssertFinishIn();
        Assert.Fail("except throw exception from interval");
      } catch (Exception ex) {
        Assert.AreEqual(ex.Message, "ex");
      }
    }

  }

}
