using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Midos.Domain.Test;
using Midos.Services.Http;
using Moq;
using Namei.Wcs.Api;
using Namei.Wcs.Api.Test;

namespace Namei.Wcs.Aggregates.Test
{
  [TestClass]
  public class AgcTaskServiceTest
  {
    private readonly WcsContext _context = Utils.GetDomain();

    private AgcTaskService UseService(
      IRcsService rcs = null,
      IRcsMapService rcsMap = null
    ) => new(_context, rcs, rcsMap);

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void Test_Create(bool useTypeId)
    {
      var rcsTaskCode = "100235";
      var param = AgcTaskCreate.From(
        typeId: useTypeId ? TestData.AgcTaskType.Id : 0,
        type: useTypeId ? null : TestData.AgcTaskType.Key,
        position: "000001",
        destination: "000002",
        podCode: "100000",
        priority: "5",
        taskId: "100000"
      );
      var rcsMap = Helper.UseService<IRcsMapService>(mock => mock
        .Setup(map => map .ToDataName(It.IsAny<string[]>()))
        .Returns(new string[] {
          param.Position + "a",
          param.Destination + "b"
        })
      );
      var rcs = Helper.UseService<IRcsService>(mock => mock
        .Setup(rcs => rcs
          .CreateTask(It.Is<RcsTaskCreateParams>(param =>
            param.PositionCodePath[0].PositionCode.EndsWith("a") &&
            param.PositionCodePath[1].PositionCode.EndsWith("b")
          ))
        )
        .ReturnsAsync(new RcsTaskCreateResult {
          Code = "0",
          Data = rcsTaskCode,
        })
      );
      var service = UseService(rcs, rcsMap);
      var result = service.Create(param);
      var data = _context.Find<AgcTask>(result.Id);

      Assert.AreEqual(param.TypeId, data.TypeId);
      Assert.AreEqual(param.Position, data.Position);
      Assert.AreEqual(param.Destination, data.Destination);
      Assert.AreEqual(param.PodCode, data.PodCode);
      Assert.AreEqual(param.Priority, data.Priority);
      Assert.AreEqual(param.TaskId, data.TaskId);
      Assert.AreEqual(rcsTaskCode, data.RcsTaskCode);
      Assert.AreEqual(AgcTaskStatus.Created, data.Status);
    }

    [TestMethod]
    public void Test_Create_By_Area()
    {
      var rcsTaskCode = "100235";
      var param = AgcTaskCreate.From(
        typeId: TestData.AgcTaskType.Id,
        position: "000001",
        destination: "002${04}",
        podCode: "100000",
        priority: "5",
        taskId: "100000"
      );
      var rcsMap = Helper.UseService<IRcsMapService>(
        mock => {
          mock
            .Setup(map => map.ToDataName(It.IsAny<string[]>()))
            .Returns(new string[] {
              param.Position,
              param.Destination
            });
          mock
            .Setup(map => map.GetFreeLocationCode(
              It.Is<string>(value => value == "002")
            ))
            .Returns("000002");
        }
      );
      var rcs = Helper.UseService<IRcsService>(mock => mock
        .Setup(rcs => rcs.CreateTask(
          It.Is<RcsTaskCreateParams>(param =>
            param.PositionCodePath[0].PositionCode == "000001" &&
            param.PositionCodePath[1].PositionCode == "000002"
          )
        ))
        .ReturnsAsync(new RcsTaskCreateResult {
          Code = "0",
          Data = rcsTaskCode,
        })
      );
      var service = UseService(rcs, rcsMap);
      var result = service.Create(param);
      var data = _context.Find<AgcTask>(result.Id);

      Assert.AreEqual(AgcTaskStatus.Created, data.Status);
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
        HttpPost.@event
        // HttpPost.From(
        //   url: TestData.AgcTaskType.Webhook,
        //   data: new AgcTaskService.AgcTaskCallback {
        //     Type = task.Type.Key,
        //     TaskId = task.TaskId,
        //     AgvCode = task.TaskId,
        //   }
        // )
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

    [TestMethod]
    public async Task Test_CreateTaskFromRcsApi_Position()
    {
      var param = new RcsTaskCreateParams {
        PositionCodePath = new() {
          new() { PositionCode = "100000", Type = "0" },
          new() { PositionCode = "100001", Type = "0" },
        }
      };
      var rcsMap = Helper.UseService<IRcsMapService>(mock => {
        mock.Setup(map => map.ToDataName(It.IsAny<string[]>()))
          .Returns(new string[] { "100000", "100001" });
      });
      var rcs = Helper.UseService<IRcsService>(mock => mock
        .Setup(rcs => rcs.CreateTask(
          It.Is<RcsTaskCreateParams>(result =>
            result.PositionCodePath[0].PositionCode == "100000" &&
            result.PositionCodePath[1].PositionCode == "100001"
          )
        ))
        .ReturnsAsync(new RcsTaskCreateResult () {
          Code = "0",
          Message = "success"
        })
      );

      var service = UseService(rcs, rcsMap);
      var result = await service.CreateTaskFromRcsApiAsync(param);

      Assert.IsNotNull(result);
      Assert.AreEqual("0", result.Code);
      Assert.AreEqual("success", result.Message);
    }

    [TestMethod]
    public async Task Test_CreateTaskFromRcsApi_Area()
    {
      var param = new RcsTaskCreateParams {
        PositionCodePath = new() {
          new() { PositionCode = "100000", Type = "0" },
          new() { PositionCode = "100${04}", Type = "0" },
        }
      };
      var rcsMap = Helper.UseService<IRcsMapService>(mock => {
        mock.Setup(map => map.ToDataName(It.IsAny<string[]>()))
          .Returns(new string[] { "100000", "100${04}" });
        mock.Setup(map => map.GetFreeLocationCode("100"))
          .Returns("200001");
      });
      var rcs = Helper.UseService<IRcsService>(mock => mock
        .Setup(rcs => rcs.CreateTask(
          It.Is<RcsTaskCreateParams>(result =>
            result.PositionCodePath[0].PositionCode == "100000" &&
            result.PositionCodePath[1].PositionCode == "200001"
          )
        ))
        .ReturnsAsync(new RcsTaskCreateResult () {
          Code = "0",
          Message = "success"
        })
      );

      var service = UseService(rcs, rcsMap);
      var result = await service.CreateTaskFromRcsApiAsync(param);

      Assert.IsNotNull(result);
      Assert.AreEqual("0", result.Code);
      Assert.AreEqual("success", result.Message);
    }
  }
}
