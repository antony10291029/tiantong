using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace Midos.Services.Abstract.Test
{
  public class TestIntervalService: IntervalService
  {
    public int Count = 0;

    public int Delay = 0;

    public TestIntervalService()
    {
      Time = 1;
    }

    protected override Task HandleJob(CancellationToken _)
    {
      Count++;

      return Task.CompletedTask;
    }
  }

  public class TestStoppingIntervalService: IntervalService
  {
    protected override async Task HandleJob(CancellationToken stoppingToken)
    {
      await Task.Delay(10000, stoppingToken);
    }
  }

  public class TestUnstoppingIntervalService: IntervalService
  {
    public CancellationTokenSource StoppingTokenSource = new();

    protected override async Task HandleJob(CancellationToken _)
    {
      await Task.Delay(10000, StoppingTokenSource.Token);
    }
  }

  [TestClass]
  public class IntervalHostedServiceTest
  {
    [TestMethod]
    public async Task Test_HandleJob()
    {
      var service = new TestIntervalService();
      var stoppingToken = new CancellationToken();

      await service.StartAsync(stoppingToken);
      await Task.Delay(1);
      await service.StopAsync(stoppingToken);

      Assert.IsTrue(service.Count > 0);
    }

    [TestMethod]
    public async Task Test_Stopping()
    {
      var service = new TestStoppingIntervalService();
      var stoppingToken = new CancellationToken();

      await service.StartAsync(stoppingToken);

      var task = service.StopAsync(stoppingToken);

      await Task.Delay(1);

      Assert.IsTrue(task.IsCompleted);
    }

    [TestMethod]
    public async Task Task_Unstopping()
    {
      var service = new TestUnstoppingIntervalService();
      var stoppingToken = new CancellationToken();

      await service.StartAsync(stoppingToken);

      var task = service.StopAsync(stoppingToken);

      await Task.Delay(1);

      Assert.IsFalse(task.IsCompleted);

      service.StoppingTokenSource.Cancel();

      await Task.Delay(1);

      Assert.IsTrue(task.IsCompleted);
    }
  }
}
