using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Midos.Domain.Test;
using Midos.Services.Http;
using Midos.Test;
using Moq;
using Namei.Wcs.Api;
using Namei.Wcs.Api.Test;
using System.Linq;

namespace Namei.Wcs.Aggregates.Test
{
  [TestClass]
  public class AgcTaskServiceTest
  {
    private readonly WcsContext _context = Utils.GetDomain();

    private AgcTaskService UseService(IRcsService rcs = null)
      => new(_context, rcs);

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void Test_Create(bool useTypeId)
    {
      var param = useTypeId
        ? AgcTaskCreate.From(
            typeId: TestData.AgcTaskType.Id,
            position: "000001",
            destination: "000002",
            podCode: "100000",
            priority: "5",
            taskId: "100000"
          )
        : AgcTaskCreate.From(
          type: TestData.AgcTaskType.Key,
          position: "000001",
          destination: "000002",
          podCode: "100000",
          priority: "5",
          taskId: "100000"
        );
      var service = UseService();
      var id = service.Create(param);
      var data = _context.Find<AgcTask>(id);

      Assert.AreEqual(param.TypeId, data.TypeId);
      Assert.AreEqual(param.Position, data.Position);
      Assert.AreEqual(param.Destination, data.Destination);
      Assert.AreEqual(param.PodCode, data.PodCode);
      Assert.AreEqual(param.Priority, data.Priority);
      Assert.AreEqual(param.TaskId, data.TaskId);
      Assert.AreEqual(AgcTaskStatus.Created, data.Status);
      AssertHelper.HasEvent(
        AgcTaskCreated.@event,
        AgcTaskCreated.From(id)
      );
    }

    [TestMethod]
    public void Test_Start()
    {
      var task = AgcTask.From(
        typeId: TestData.AgcTaskType.Id,
        position: "000001",
        destination: "000002"
      );

      _context.Add(task);
      _context.SaveChanges();

      var result = new RcsTaskCreateResult {
        data = "100000"
      };
      var rcs = Helper.UseService<IRcsService>(mock => mock
        .Setup(rcs => rcs.CreateTask(It.IsAny<RcsTaskCreateParams>()))
        .Returns(result)
      );
      var param = AgcTaskStart.From(task.Id);
      var service = UseService(rcs);

      service.Start(param);

      AssertHelper.HasEvent(
        AgcTaskStarted.@event,
        AgcTaskStarted.From(
          id: task.Id,
          taskCode: result.data
        )
      );
    }

    [TestMethod]
    public void Test_Started()
    {
      var task = AgcTask.From(
        typeId: TestData.AgcTaskType.Id,
        position: "0010000",
        destination: "0020000"
      );

      _context.Add(task);
      _context.SaveChanges();

      var param = AgcTaskStarted.From(
        id: task.Id,
        taskCode: "100000"
      );
      var service = UseService();

      service.Started(param);

      var data = _context.Find<AgcTask>(task.Id);

      Assert.AreEqual(param.TaskCode, data.RcsTaskCode);
      Assert.AreEqual(AgcTaskStatus.Started, data.Status);
    }

    [TestMethod]
    public void Test_Close()
    {
      var task = AgcTask.From(
        typeId: TestData.AgcTaskType.Id,
        position: "0010000",
        destination: "0020000"
      );

      _context.Add(task);
      _context.SaveChanges();

      var param = AgcTaskClose.From(task.Id);
      var service = UseService();

      service.Close(param);

      var data = _context.Find<AgcTask>(task.Id);

      Assert.AreEqual(AgcTaskStatus.Closed, data.Status);
    }

    [TestMethod]
    public void Test_Finish()
    {
      var task = AgcTask.From(
        typeId: TestData.AgcTaskType.Id,
        position: "0010000",
        destination: "0020000"
      );

      _context.Add(task);
      _context.SaveChanges();

      var service = UseService();
      var param = AgcTaskFinish.From(
        id: task.Id,
        agcCode: task.AgcCode
      );

      service.Finish(param);

      var data = _context.Find<AgcTask>(task.Id);

      Assert.AreEqual(AgcTaskStatus.Finished, data.Status);
    }

    [TestMethod]
    public void Test_Finished()
    {
      var task = AgcTask.From(
        typeId: TestData.AgcTaskType.Id,
        position: "0010000",
        destination: "0010000",
        taskId: "200000"
      );

      task.Start("100000");
      task.Finish("01");
      _context.Add(task);
      _context.SaveChanges();

      var service = UseService();
      var param = AgcTaskFinished.From(task.Id);

      service.Finished(param);

      AssertHelper.HasEvent(
        HttpPost.@event,
        HttpPost.From(
          url: TestData.AgcTaskType.Webhook,
          data: new AgcTaskService.AgcTaskCallback {
            Type = task.Type.Key,
            TaskId = task.TaskId,
            AgvCode = task.TaskId,
          }
        )
      );
    }

    [TestMethod]
    public void Test_FindByTaskCode()
    {
      var task = AgcTask.From(
        typeId: TestData.AgcTaskType.Id,
        position: "0010000",
        destination: "0020000"
      );

      task.Start("100020");
      _context.Add(task);
      _context.SaveChanges();

      var service = UseService();
      var data = service.FindByTaskCode(task.RcsTaskCode);

      Assert.AreEqual(task.Id, data.Id);
    }
  }
}
