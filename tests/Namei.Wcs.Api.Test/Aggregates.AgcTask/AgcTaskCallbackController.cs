using System.Linq.Expressions;
using DBCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Midos.Domain.Test;
using Namei.Wcs.Api.Test;

namespace Namei.Wcs.Aggregates
{
  [TestClass]
  public class AgvTaskCallbackControllerTest
  {
    [TestMethod]
    [DataRow("test.update.start", "start")]
    [DataRow("test.update.finish", "finish")]
    public void Test(string taskCode, string method)
    {
      var context = Utils.GetDomain();
      var task = AgcTask.From(
        typeId: TestData.AgcTaskType.Id,
        position: "100100",
        destination: "100200",
        podCode: "100"
      );

      task.Start(taskCode);
      context.Add(task);
      context.SaveChanges();

      var param = new AgcTaskCallbackController.AgcCallbackParams {
        TaskCode = taskCode,
        RobotCode = "10",
        Method = method
      };
      var logger = new Namei.Wcs.Api.Logger(context);
      var agcService = new AgcTaskService(context, null, null);
      var controller = new AgcTaskCallbackController(agcService, logger);
      var result = controller.RcsCallback(param);
      var data = context.Find<AgcTask>(task.Id);

      if (method == "finish") {
        Assert.AreEqual(AgcTaskStatus.Finished, data.Status);
      } else {
        Assert.AreNotEqual(AgcTaskStatus.Finished, data.Status);
      }

      Assert.AreEqual(result.GetStatusCode(), 200);
    }
  }
}
