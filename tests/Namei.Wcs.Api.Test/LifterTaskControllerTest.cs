using System.Reflection.Emit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Midos.Domain.Test;
using Moq;
using System;
using System.Threading.Tasks;

namespace Namei.Wcs.Api.Test
{
  [TestClass]
  public class LifterEventControllerTest
  {
    private DomainContext _domain = new TestDomainContext();

    private IWmsService UseWms(Action<Mock<IWmsService>> useWms)
      => Helper.UseService<IWmsService>(useWms);

    private ILifterLogger UseLogger(Action<Mock<ILifterLogger>> useLogger)
      => Helper.UseService<ILifterLogger>(useLogger);

    private ILifterService UseLifter(Action<Mock<ILifterService>> useLifter)
      => Helper.UseService<ILifterService>(useLifter);

    private ILifterServiceFactory UseLifters(Action<Mock<ILifterServiceFactory>> useMock)
      => Helper.UseService<ILifterServiceFactory>(useMock);

    private LifterEventController UseController(
      ILifterServiceFactory lifters = null,
      IWmsService wms = null
    ) => new LifterEventController(UseLogger(null), lifters, wms);

    [TestMethod]
    public void Test_Import_Create()
    {
      var isImported = false;
      var param = LifterTaskImported.From(
        lifterId: "1",
        floor: "1",
        taskCode: "test.imported.create",
        barcode: "10000",
        destination: "2"
      );
      var lifter = UseLifter(mock => mock
        .Setup(lifter => lifter.Import(param.LifterId, param.Destination, param.Barcode))
        .Callback(() => isImported = true)
      );
      var lifters = UseLifters(mock => mock
        .Setup(lifters => lifters.Get(param.LifterId))
        .Returns(lifter)
      );
      var controller = UseController(lifters);

      controller.HandleTaskImported(param);

      Assert.AreEqual(isImported, true);
    }

    [TestMethod]
    [DataRow("100000", "0")]
    [DataRow("100000", "4")]
    public async Task Test_Scanned(string barcode, string destination)
    {
      var isWmsCalled = false;
      var isDestinationWritten = false;
      var param = new LifterTaskScannedEvent("1", "4");
      var wms = UseWms(mock => {
        mock.Setup(wms => wms.GetPalletInfo(barcode))
          .ReturnsAsync(new PalletInfo() { Destination = destination })
          .Callback(() => isWmsCalled = true);
      });
      var lifter = UseLifter(mock => {
        mock.Setup(lifter => lifter.GetPalletCode(param.Floor))
          .Returns(barcode);

        mock.Setup(lifter => lifter.GetDestination(param.Floor))
          .Returns(destination);

        mock.Setup(lifter => lifter.SetDestination(param.Floor, destination))
          .Callback(() => isDestinationWritten = true);
      });
      var lifters = UseLifters(mock => mock
        .Setup(lifters => lifters.Get(param.LifterId))
        .Returns(lifter)
      );
      var controller = UseController(lifters, wms);

      await controller.HandleTaskScanned(param);

      if (destination == "0") {
        Assert.IsTrue(isWmsCalled);
        Assert.IsTrue(isDestinationWritten);
      } else {
        Assert.IsTrue(!isWmsCalled);
        Assert.IsTrue(!isDestinationWritten);
      }
    }

    [TestMethod]
    [DataRow(5)]
    [DataRow(15)]
    public async Task Test_Exported(int seconds)
    {
      var floor = "1";
      var lifterId = "1";
      var barcode = "100000";
      var destination = "4";
      var taskCode = "123456";
      var param = new LifterTaskExported {
        LifterId = lifterId,
        Floor = floor
      };
      var isRequested = false;
      var lifter = UseLifter(mock => {
        mock.Setup(lifter => lifter.ExportedAt)
          .Returns(new System.Collections.Generic.Dictionary<string, DateTime> {
            { floor, DateTime.Now.AddSeconds(-seconds) }
          });
        
        mock.Setup(lifter => lifter.GetPalletCode(floor))
          .Returns(barcode);
      });
      var lifters = UseLifters(mock => mock
        .Setup(lifters => lifters.Get(lifterId))
        .Returns(lifter)
      );
      var wms = UseWms(mock => {
        mock.Setup(wms => wms.GetPalletInfo(barcode))
          .ReturnsAsync(new PalletInfo { Destination = destination,  TaskId = taskCode});
        mock.Setup(wms => wms.RequestPicking(lifterId, floor, barcode, taskCode))
          .Callback(() => isRequested = true);
      });
      var controller = UseController(lifters, wms);

      await controller.HandleTaskExported(param);

      if (seconds < 10) {
        Assert.IsTrue(!isRequested);
      } else {
        Assert.IsTrue(isRequested);
      }
    }

    [TestMethod]
    public void Test_Taken()
    {
      var floor = "1";
      var lifterId = "1";
      var isPicked = false;
      var param = new LifterTaskTaken {
        LifterId = lifterId,
        Floor = floor
      };
      var lifter = UseLifter(mock => mock
        .Setup(lifter => lifter.SetPickuped(floor, true))
        .Callback(() => isPicked = true)
      );
      var lifters = UseLifters(mock => mock
        .Setup(lifters => lifters.Get(lifterId))
        .Returns(lifter)
      );
      var controller = UseController(lifters);

      controller.HandleTaskTaken(param);

      Assert.IsTrue(isPicked);
    }
  }
}
