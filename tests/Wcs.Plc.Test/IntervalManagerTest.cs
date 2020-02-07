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

    [TestCase(0, 0, 1)]
    [TestCase(1, 1, 1)]
    [TestCase(10, 10, 10)]
    public void TestRun(int n, int m, int time)
    {
      var times = 0;
      var manager = GetManager(m, time, () => {
        Interlocked.Increment(ref times);
      });

      for (var i = 0; i < n; i++) {
        manager.Start().Wait();
      }

      Assert.AreEqual(n * m * time, times);
    }

    [TestCase(0, 0, 0)]
    [TestCase(1, 1, 1)]
    [TestCase(10, 10, 500)]
    public void TestStop(int n, int m, int time)
    {
      var times = 0;
      var manager = new IntervalManager();

      for (var i = 0; i < n; i++) {
        var interval = new Interval();
        interval.SetTime(500);
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
      manager.Wait();

      Assert.IsTrue(true);
    }

    [TestCase(1)]
    [TestCase(10)]
    [TestCase(100)]
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

      Assert.IsTrue(true);
    }
  }
}
