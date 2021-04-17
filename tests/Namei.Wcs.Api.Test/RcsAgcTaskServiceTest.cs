using Microsoft.VisualStudio.TestTools.UnitTesting;
using Midos.Domain.Test;
using Moq;
using Namei.Wcs.Aggregates;
using System;

namespace Namei.Wcs.Api.Test
{
  [TestClass]
  public class RcsAgcTaskServiceTest
  {
    private static RcsAgcTaskCreate _createParam = RcsAgcTaskCreate.From(
      taskType: "test",
      position: "000001",
      destination: "000002",
      podCode: "100000",
      comment: "comment",
      orderType: "test",
      orderId: 100
    );

    private RcsAgcTask _task = RcsAgcTask.From(_createParam);

    private WcsContext _domain = new TestDomainContext();

    private IRcsService UseRcs(Action<Mock<IRcsService>> mockRcs)
      => Helper.UseService<IRcsService>(mockRcs);

    private IRcsAgcTaskService UseRcsTask(IRcsService rcs = null)
      => new RcsAgcTaskService(_domain, rcs);

    [TestMethod]
    public void Test_Create()
    {
      var service = UseRcsTask();

      var id = service.Create(_createParam);

      var task = _domain.Set<RcsAgcTask>().Find(id);

      Assert.AreEqual(task.TaskType, _createParam.TaskType);
      Assert.AreEqual(task.Position, _createParam.Position);
      Assert.AreEqual(task.Destination, _createParam.Destination);
      Assert.AreEqual(task.PodCode, _createParam.PodCode);
      Assert.AreEqual(task.Comment, _createParam.Comment);
      Assert.AreEqual(task.OrderType, _createParam.OrderType);
      Assert.AreEqual(task.OrderId, _createParam.OrderId);
      AssertHelper.HasEvent(
        name: RcsAgcTaskCreated.Message,
        data: RcsAgcTaskCreated.From(id: task.Id)
      );
    }

    [TestMethod]
    public void Test_Start()
    {
      var rcs = UseRcs(mock => mock
        .Setup(rcs => rcs.CreateTask(It.IsAny<RcsTaskCreateParams>()))
        .Returns(new RcsTaskCreateResult {
          code = "0",
          data = "000001"
        })
      );
      var service = UseRcsTask(rcs);

      _domain.Add(_task);
      _domain.SaveChanges();

      service.Start(RcsAgcTaskStart.From(
        id: _task.Id,
        isEnforced: false
      ));

      AssertHelper.HasEvent(
        name: RcsAgcTaskStarted.Message,
        data: RcsAgcTaskStarted.From(
          id: _task.Id,
          taskCode: "000001"
        )
      );
    }

    [TestMethod]
    public void Test_Started()
    {
      var service = UseRcsTask();

      _domain.Add(_task);
      _domain.SaveChanges();

      service.Started(RcsAgcTaskStarted.From(
        id: _task.Id,
        taskCode: "000001"
      ));

      Assert.AreEqual(_task.TaskCode, "000001");
      Assert.AreEqual(_task.Status, RcsAgcTaskStatus.Started);
    }

    [TestMethod]
    public void Test_Close()
    {
      var service = UseRcsTask();

      _task.Start("000001");
      _domain.Add(_task);
      _domain.SaveChanges();

      service.Close(RcsAgcTaskClose.From(
        id: _task.Id
      ));

      Assert.AreEqual(_task.Status, RcsAgcTaskStatus.Closed);
    }

    [TestMethod]
    public void Test_Finish()
    {
      var service = UseRcsTask();

      _task.Start("000001");
      _domain.Add(_task);
      _domain.SaveChanges();

      service.Finish(RcsAgcTaskFinish.From(
        id: _task.Id,
        agcCode: "100"
      ));

      AssertHelper.HasEvent(
        name: RcsAgcTaskFinished.Message,
        data: RcsAgcTaskFinished.From(id: _task.Id)
      );
    }

    [TestMethod]
    public void Test_Finished()
    {
      var service = UseRcsTask();

      _task.Start("000001");
      _task.Finish("100");
      _domain.Add(_task);
      _domain.SaveChanges();

      service.Finished(RcsAgcTaskFinished.From(
        id: _task.Id
      ));

      AssertHelper.HasEvent(
        name: RcsAgcTaskOrderFinished.Message(_task.OrderType),
        data: RcsAgcTaskOrderFinished.From(
          id: _task.Id,
          orderId: _task.OrderId,
          agcCode: _task.AgcCode,
          podCode: _task.PodCode
        )
      );
    }

    [TestMethod]
    public void Test_FindByTaskCode()
    {
      var service = UseRcsTask();

      _task.Start("test.find.by.task.code");
      _domain.Add(_task);
      _domain.SaveChanges();

      var task = service.FindByTaskCode(_task.TaskCode);

      Assert.AreEqual(task.Id, _task.Id);
      Assert.AreEqual(task.TaskCode, _task.TaskCode);
    }

  }
}
