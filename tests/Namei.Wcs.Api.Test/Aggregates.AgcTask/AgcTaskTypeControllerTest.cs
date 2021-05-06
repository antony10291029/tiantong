using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Midos.Domain;
using Namei.Wcs.Api;
using Namei.Wcs.Api.Test;
using System.Linq;

namespace Namei.Wcs.Aggregates.Test
{
  using Controller = AgcTaskTypeController;

  [TestClass]
  public class AgcTaskTypeControllerTest
  {
    private readonly static WcsContext _context = Utils.GetDomain();

    private static Controller UseController()
      => new(_context);

    [TestMethod]
    public void Test_Create()
    {
      var controller = UseController();
      var param =  new Controller.CreateParams {
        Key = "test.create",
        Name = "test.name",
        Method = "wcs.move",
        Webhook = "http://localhost",
        IsEnabled = true,
      };
      var result = controller.Create(param);
      var data = Utils.GetDomain()
        .Set<AgcTaskType>()
        .First<AgcTaskType>(
          type => type.Key == param.Key
        );

      Assert.AreEqual(param.Key, data.Key);
      Assert.AreEqual(param.Name, data.Name);
      Assert.AreEqual(param.Method, data.Method);
      Assert.AreEqual(param.Webhook, data.Webhook);
      Assert.AreEqual(result.GetStatusCode(), 201);
    }

    [TestMethod]
    public void Test_Create_Duplicate()
    {
      var type = AgcTaskType.From(
        key: "test.create.duplicate",
        name: "test.name",
        method: "wcs.move",
        webhook: "http://localhost",
        isEnabled: true
      );

      _context.Add(type);
      _context.SaveChanges();

      var param =  new Controller.CreateParams {
        Key = type.Key,
        Name = type.Name,
        Method = type.Method,
        Webhook = type.Webhook,
        IsEnabled = true
      };
      var controller = UseController();
      var result = controller.Create(param);

      Assert.AreEqual(result.GetStatusCode(), 400);
    }

    [TestMethod]
    public void Test_Update()
    {
      var type = AgcTaskType.From(
        key: "test.update",
        name: "test.name",
        method: "wcs.move",
        webhook: "http://localhost",
        isEnabled: true
      );

      _context.Add(type);
      _context.SaveChanges();

      var param = new Controller.CreateParams {
        Id = type.Id,
        Key = "test.update.updated",
        Name = "test.name.updated",
        Method = "wcs.move.lock",
        Webhook = "http://localhost:8080"
      };
      var controller = UseController();
      var result = controller.Update(param);
      var data = _context.Find<AgcTaskType>(type.Id);

      Assert.AreEqual(param.Key, data.Key);
      Assert.AreEqual(param.Name, data.Name);
      Assert.AreEqual(param.Method, data.Method);
      Assert.AreEqual(param.Webhook, data.Webhook);
      Assert.AreEqual(result.GetStatusCode(), 201);
    }

    [TestMethod]
    public void Test_Delete()
    {
      var type = AgcTaskType.From(
        key: "test.delete",
        name: "test.name",
        method: "wcs.move",
        webhook: "http://localhost",
        isEnabled: true
      );

      _context.Add(type);
      _context.SaveChanges();

      var param = new Controller.DeleteParams {
        Id = type.Id
      };
      var controller = UseController();
      var result = controller.Delete(param);
      var data = _context.Find<AgcTaskType>(type.Id);
      
      Assert.IsNull(data);
      Assert.AreEqual(result.GetStatusCode(), 201);
    }

    [TestMethod]
    public void Test_All()
    {
      var count = _context.Set<AgcTaskType>().Count();
      var controller = UseController();
      var data = controller.All(new QueryParams());

      Assert.AreEqual(count, data.Keys.Length);
    }
  }
}
