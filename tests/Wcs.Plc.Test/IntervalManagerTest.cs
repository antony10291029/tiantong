using System;
using System.Threading;
using NUnit.Framework;

namespace Wcs.Plc.Test
{
  [TestFixture]
  public class IntervalManagerTest
  {
    private IntervalManager GetManager(int n, int time, Action handler)
    {
      var manager = new IntervalManager();
      for (var i = 0; i < n; i++) {
        var interval = new Interval();
        interval.SetTimes(time);
        interval.SetHandler(handler);
        manager.Add(interval);
      }

      return manager;
    }

    [Test]
    public void TestTryWait()
    {
      var manager = new IntervalManager();

      manager.Add(new Interval(() => {}, 100));

      try {
        manager.Start().WaitAsync().AssertFinishIn(1);
        Assert.Fail("except to throw exception when IntervalManager.TryWait is timeout");
      } catch {};
    }

    [TestCase(0, 0, 1)]
    [TestCase(1, 1, 1)]
    [TestCase(10, 10, 1)]
    public void TestRun(int n, int m, int time)
    {
      var times = 0;
      var manager = GetManager(m, time, () => {
        Interlocked.Increment(ref times);
      });

      for (var i = 0; i < n; i++) {
        manager.Start().WaitAsync().AssertFinishIn();
      }

      Assert.AreEqual(n * m * time, times);
    }

    [TestCase(0, 0, 0)]
    [TestCase(1, 1, 1)]
    [TestCase(10, 10, 1)]
    public void TestStop(int n, int m, int time)
    {
      var times = 0;
      var manager = new IntervalManager();

      for (var i = 0; i < n; i++) {
        var interval = new Interval();
        interval.SetTime(time);
        interval.SetHandler(() => times++);
        manager.Add(interval);
      }

      for (var i = 0; i < m; i++) {
        manager.Start();
        manager.Stop();
        Assert.IsTrue(true);
      }
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(5)]
    public void TestRemove(int n)
    {
      var times = 0;
      var intervals = new Interval[n];
      var manager = new IntervalManager();

      for (var i = 0; i < n; i++) {
        intervals[i] = new Interval();
        intervals[i].SetHandler(() => times++);
        manager.Add(intervals[i]);
      }

      manager.Start();
      for (var i = 0; i < n; i++) {
        var interval = intervals[i];
        manager.Remove(interval);
      }
      manager.WaitAsync().AssertFinishIn();

      Assert.IsTrue(true);
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
        interval.SetHandler(() => times++);
        manager.Add(interval);
      }

      manager.Start();
      manager.Clear();
      manager.WaitAsync().AssertFinishIn();

      Assert.IsTrue(true);
    }
  }
}
