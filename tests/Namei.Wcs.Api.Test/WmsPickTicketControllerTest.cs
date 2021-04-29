// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using Midos.Domain.Test;
// using Midos.Services.Http;
// using Namei.Wcs.Aggregates;

// namespace Namei.Wcs.Api.Test
// {
//   [TestClass]
//   public class WcsPickTicketControllerTest
//   {
//     private IAppConfig _config = Helper.UseService<IAppConfig>(mock => {
//       mock.Setup(config => config.IsProduction).Returns(false);
//       mock.Setup(config => config.IsDevelopment).Returns(true);
//     });

//     private WmsPickTicketTaskController UseController()
//       => new WmsPickTicketTaskController(_config, Utils.GetDomain());

//     [TestMethod]
//     public void Test_Start()
//     {
//       var controller = UseController();
//       var param = new WmsPickTicketTaskController
//         .StartParams {
//           TaskId = 1,
//           Position = "000001",
//           Destination = "000002",
//           PalletCode = "000003"
//         };

//       controller.Start(param);

//       AssertHelper.HasEvent(
//         RcsAgcTaskCreate.Message,
//         RcsAgcTaskCreate.From(
//           taskType: RcsAgcTaskMethod.Carry,
//           position: param.Position,
//           destination: param.Destination,
//           priority: "",
//           podCode: param.PalletCode,
//           comment: "",
//           orderType: "wms.export.carry",
//           orderId: param.TaskId
//         )
//       );
//     }

//     [TestMethod]
//     public void TestFinish()
//     {
//       var controller = UseController();
//       var param = RcsAgcTaskOrderFinished.From(
//         id: 100,
//         orderId: 1000,
//         agcCode: "100",
//         podCode: "200"
//       );

//       controller.Finished(param);

//       AssertHelper.HasEvent(
//         HttpPost.Event,
//         HttpPost.From(
//           url: "http://localhost:5300/wms/pick-ticket-tasks/finish",
//           data: new { Id = param.OrderId }
//         )
//       );
//     }
//   }
// }
