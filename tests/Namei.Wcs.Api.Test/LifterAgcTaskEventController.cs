using Microsoft.VisualStudio.TestTools.UnitTesting;
using Midos.Domain.Test;
using Namei.Wcs.Aggregates;
using System.Text.Json;

namespace Namei.Wcs.Api.Test
{
  [TestClass]
  public class LifterAgcTaskEventControllerTest
  {
    private WcsContext _context = Utils.GetDomain();

    private LifterAgcTaskEventController UseController()
      => new LifterAgcTaskEventController(_context);

    [TestMethod]
    public void Test_Created()
    {
      var controller = UseController();
      var param = LifterAgcTaskEvent.From(100);

      controller.Created(param);

      AssertHelper.HasEvent(
        LifterAgcTaskEvent.Started,
        LifterAgcTaskEvent.From(param.Id)
      );
    }

    [TestMethod]
    public void Test_Started()
    {
      var type = TestData.LifterAgcTaskType;
      var task = LifterAgcTask.From(
        type: type,
        orderId: 100,
        palletCode: "100000",
        position: "100000",
        destination: "100000",
        lifterId: "0"
      );

      _context.Add(task);
      _context.SaveChanges();

      var controller = UseController();
      var param = LifterAgcTaskEvent.From(task.Id);

      controller.Started(param);

      var data = _context.Find<LifterAgcTask>(param.Id);

      Assert.AreEqual(data.Status, LifterAgcTaskStatus.Exported);
      AssertHelper.HasEvent(LifterAgcTaskEvent.Exported, param);
    }

    [TestMethod]
    public void Test_Export()
    {
      var type = TestData.LifterAgcTaskType;
      var task = LifterAgcTask.From(
        type: type,
        orderId: 101,
        palletCode: "100000",
        position: "100000",
        destination: "100000",
        lifterId: "1"
      );

      task.Export();
      _context.Add(task);
      _context.SaveChanges();

      var controller = UseController();
      var param = LifterAgcTaskEvent.From(task.Id);

      controller.Export(param);

      AssertHelper.HasEvent(
        RcsAgcTaskCreate.Message,
        RcsAgcTaskCreate.From(
          taskType: RcsAgcTaskType.Carry,
          position: task.Position,
          destination: task.Destination,
          priority: "",
          podCode: task.PalletCode,
          comment: "",
          orderType: "export",
          orderId: task.Id
        )
      );
    }

    [TestMethod]
    public void Test_Finish()
    {
      var type = TestData.LifterAgcTaskType;
      var task = LifterAgcTask.From(
        type: type,
        orderId: 102,
        palletCode: "100000",
        position: "100000",
        destination: "100000",
        lifterId: "1"
      );

      _context.Add(task);
      _context.SaveChanges();

      var controller = UseController();
      var param = RcsAgcTaskOrderFinished.From(
        id: 1,
        orderId: task.Id,
        agcCode: "100",
        podCode: "500"
      );

      controller.Finish(param);

      var data = _context.Find<LifterAgcTask>(task.Id);

      Assert.AreEqual(data.Status, LifterAgcTaskStatus.Finished);
      AssertHelper.HasEvent(
        LifterAgcTaskEvent.Finished,
        LifterAgcTaskEvent.From(task.Id)
      );
    }

    [TestMethod]
    public void Test_Finished()
    {
      var type = TestData.LifterAgcTaskType;
      var task = LifterAgcTask.From(
        type: type,
        orderId: 103,
        palletCode: "100000",
        position: "100000",
        destination: "100000",
        lifterId: "1"
      );

      _context.Add(task);
      _context.SaveChanges();

      var controller = UseController();
      var param = LifterAgcTaskEvent.From(task.Id);

      controller.Finished(param);

      AssertHelper.HasEvent(
        WebHookPost.Message,
        new WebHookPost(
          url: task.Type.WebHook,
          data: JsonSerializer.Serialize(new { OrderId = task.OrderId })
        )
      );
    }
  }
}
