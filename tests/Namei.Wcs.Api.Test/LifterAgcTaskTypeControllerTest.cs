using Midos.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Namei.Wcs.Aggregates;
using System.Linq;

namespace Namei.Wcs.Api.Test
{
  [TestClass]
  public class LifterAgcTaskTypeControllerTest
  {
    private WcsContext _context = Utils.GetDomain();

    private LifterAgcTaskTypeController UseController()
      => new LifterAgcTaskTypeController(_context);

    [TestMethod]
    public void Test_Create()
    {
      var param = new LifterAgcTaskTypeController
        .CreateParams {
          Key = "test.create",
          Name = "test_name",
          WebHook = "test_webhook"
        };
      var controller = UseController();
      var result = controller.Create(param);
      var data = _context
        .Set<LifterAgcTaskType>()
        .FirstOrDefault(task => task.Key == param.Key);

      Assert.AreEqual(data.Key, param.Key);
      Assert.AreEqual(data.Name, param.Name);
      Assert.AreEqual(data.WebHook, param.WebHook);
    }

    [TestMethod]
    public void Test_Update()
    {
      var type = new LifterAgcTaskType(
        key: "test.update",
        name: "test.name",
        webHook: "test.webhook"
      );

      _context.Add(type);
      _context.SaveChanges();

      var param = new LifterAgcTaskTypeController
        .UpdateParams {
          Id = type.Id,
          Key = "test.updated",
          Name = "test.name.updated",
          WebHook = "test.webhook.updated"
        };
      var controller = UseController();
      var result = controller.Update(param);

      var data = _context.Find<LifterAgcTaskType>(param.Id);

      Assert.AreEqual(data.Key, param.Key);
      Assert.AreEqual(data.Name, param.Name);
      Assert.AreEqual(data.WebHook, param.WebHook);
    }

    [TestMethod]
    public void Test_Remove()
    {
      var type = new LifterAgcTaskType(
        key: "test.delete",
        name: "test.name",
        webHook: "test.webhook"
      );

      _context.Add(type);
      _context.SaveChanges();

      var param  = new LifterAgcTaskTypeController
        .DeleteParams { Id = type.Id };
      var controller = UseController();
      var result = controller.Delete(param);

      var data = _context.Find<LifterAgcTaskType>(param.Id);

      Assert.IsNull(data);
    }

    [TestMethod]
    public void Test_Paginate()
    {
      var controller = UseController();
      var param = new QueryParams {};
      var result = controller.Paginate(param);
      var total = _context.Set<LifterAgcTaskType>().Count();

      Assert.AreEqual(result.Total, total);
    }
  }
}
