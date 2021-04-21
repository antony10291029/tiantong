using Microsoft.VisualStudio.TestTools.UnitTesting;
using Midos.Domain.Test;
using Namei.Wcs.Aggregates;
using System.Linq;

namespace Namei.Wcs.Api.Test
{
  [TestClass]
  public class LifterAgcTaskControllerTest
  {
    public static LifterAgcTaskType TestType = new LifterAgcTaskType(
      key: "test.lifter.agc.tasks",
      name: "name",
      webHook: "webhook"
    );

    public WcsContext _context = Utils.GetDomain();

    private LifterAgcTaskController UseController()
      => new LifterAgcTaskController(_context);

    [TestMethod]
    public void Test_Create()
    {
      var param = new LifterAgcTaskController
        .CreateParams {
          OrderId = "test.create",
          Position = "000001",
          Destination = "100000",
          LifterId = "1",
          Type = TestType.Key
        };
      var controller = UseController();
      var result = controller.Create(param);
      var data = _context.Set<LifterAgcTask>()
        .FirstOrDefault(task => task.OrderId == param.OrderId);

      Assert.AreEqual(data.TypeId, TestType.Id);
      Assert.AreEqual(data.OrderId, param.OrderId);
      Assert.AreEqual(data.Position, param.Position);
      Assert.AreEqual(data.Destination, param.Destination);
      Assert.AreEqual(data.LifterId, param.LifterId);
      Assert.AreEqual(data.Status, LifterAgcTaskStatus.Created);
      AssertHelper.HasEvent(
        LifterAgcTaskEvent.Created,
        LifterAgcTaskEvent.From(data.Id)
      );
    }

    [TestMethod]
    public void Test_Close()
    {
      var task = LifterAgcTask.From(
        type: TestType,
        orderId: "test.close",
        palletCode: "000001",
        position: "100000",
        destination: "200000",
        lifterId: "0"
      );
      _context.Add(task);
      _context.SaveChanges();

      var param = new LifterAgcTaskController
        .CloseParams { Id = task.Id };
      var controller = UseController();
      var result = controller.Close(param);
      var data = _context.Find<LifterAgcTask>(param.Id);

      Assert.AreEqual(data.Id, task.Id);
      Assert.AreEqual(data.Status, LifterAgcTaskStatus.Closed);
      AssertHelper.HasEvent(
        LifterAgcTaskEvent.Closed,
        LifterAgcTaskEvent.From(task.Id)
      );
    }
  }
}
