using Microsoft.VisualStudio.TestTools.UnitTesting;
using Midos.Domain.Test;
using Namei.Wcs.Api;

namespace Namei.Wcs.Aggregates
{
  [TestClass]
  public class LifterServiceTest
  {
    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void TestHandleImported(bool IsBarcodeValid)
    {
      var lifterId = "";
      var floor = "";
      var barcode = "";
      var destination = "";
      var data = "";
      var from = "";
      var isAddCalled = false;
      var isSaveCalled = false;
      var isCommandCalled = false;
      var command = Helper.UseService<ILifterCommandService>(mock => {
        mock.Setup(command => command.Import(lifterId, floor, barcode, destination))
          .Callback(() => isCommandCalled = true);
        mock.Setup(command => command.IsBarcodeValid(barcode))
          .Returns(IsBarcodeValid);
      });
      var repository = Helper.UseService<ILifterTaskRepository>(mock => {
        mock.Setup(repository => repository.Add(lifterId, floor, barcode, destination, data, from))
          .Callback(() => isAddCalled = true);
        mock.Setup(repository => repository.SaveChanges())
          .Callback(() => isSaveCalled = true);
      });
      var service = new LifterService(command, repository, null);

      service.HandleImported(lifterId, floor, barcode, destination, data, from);

      Assert.IsTrue(isCommandCalled);
      Assert.AreEqual(IsBarcodeValid, isAddCalled);
      Assert.AreEqual(IsBarcodeValid, isSaveCalled);
    }

    [TestMethod]
    [DataRow(true, true)]
    [DataRow(true, false)]
    [DataRow(false, true)]
    [DataRow(false, false)]
    public void TestHandleScanned(bool isDestinationValid, bool isTaskFound)
    {
      var lifterId = "";
      var floor = "";
      var barcode = "";
      var destination = "";
      var data = "";
      var from = "";
      var isAddCalled = false;
      var isSaveCalled = false;
      var isTaskSourceCalled = false;
      var isSetDestinationCalled = false;
      var task = new LifterTask(lifterId, floor, barcode, destination, data, from);
      var command = Helper.UseService<ILifterCommandService>(mock => {
        mock.Setup(command => command.GetBarcode(lifterId, floor))
          .Returns(barcode);
        mock.Setup(command => command.GetTaskDestination(lifterId, floor))
          .Returns(destination);
        mock.Setup(command => command.IsDestinationValid(destination))
          .Returns(isDestinationValid);
        mock.Setup(command => command.SetDestination(lifterId, floor, destination))
          .Callback(() => isSetDestinationCalled = true);
      });
      var repository = Helper.UseService<ILifterTaskRepository>(mock => {
        mock.Setup(repository => repository.FindFromRuntimeBarcode(barcode))
          .Returns(isTaskFound ? task : null);
        mock.Setup(repository => repository.Add(lifterId, floor, barcode, destination, data, from))
          .Returns(task)
          .Callback(() => isAddCalled = true);
        mock.Setup(repository => repository.SaveChanges())
          .Callback(() => isSaveCalled = true);
      });
      var taskSource = Helper.UseService<ILifterTaskSourceService>(mock => {
        mock.Setup(source => source.FindFromBarcode(barcode))
          .Returns(new LifterTaskSource { Destination = destination, Data = data, From = from })
          .Callback(() => isTaskSourceCalled = true);
      });
      var service = new LifterService(command, repository, taskSource);

      service.HandleScanned(lifterId, floor);

      Assert.AreEqual(!isTaskFound, isAddCalled);
      Assert.AreEqual(!isTaskFound, isSaveCalled);
      Assert.AreEqual(!isTaskFound, isTaskSourceCalled);
      Assert.AreEqual(!isDestinationValid, isSetDestinationCalled);
    }

    [TestMethod]
    [DataRow(true, true)]
    [DataRow(true, false)]
    [DataRow(false, true)]
    [DataRow(false, false)]
    public void TestHandleExport(bool isTaskFound, bool isSourceFound)
    {
      var lifterId = "";
      var floor = "";
      var barcode = "";
      var destination = "";
      var data = "";
      var from = "";
      var isSaveCalled = false;
      var isRequestCalled = false;
      var task = new LifterTask(lifterId, floor, barcode, destination, data, from);
      var taskSource = new LifterTaskSource { Destination = destination, Data = data, From = from };
      var command = Helper.UseService<ILifterCommandService>(mock => {
        mock.Setup(command => command.GetBarcode(lifterId, barcode))
          .Returns(barcode);
      });
      var repository = Helper.UseService<ILifterTaskRepository>(mock => {
        mock.Setup(repository => repository.SaveChanges())
          .Callback(() => isSaveCalled = true);
        mock.Setup(repository => repository.FindFromRuntimeBarcode(barcode))
          .Returns(isTaskFound ? task : null);
      });
      var source = Helper.UseService<ILifterTaskSourceService>(mock => {
        mock.Setup(source => source.FindFromBarcode(barcode))
          .Returns(isSourceFound ? taskSource : null);
        mock.Setup(source => source.RequestToPick(lifterId, floor, barcode, data, from))
          .Callback(() => isRequestCalled = true);
      });
      var service = new LifterService(command, repository, source);

      service.HandleExported(lifterId, floor);

      Assert.AreEqual(isTaskFound, isSaveCalled);
      Assert.AreEqual(isTaskFound, task.Status == LifterTaskStatus.Exported);
      Assert.AreEqual(isTaskFound || isSourceFound, isRequestCalled);
    }

    [TestMethod]
    [DataRow(true, true)]
    [DataRow(false, true)]
    [DataRow(true, false)]
    [DataRow(false, false)]
    public void TestHandleTaken(bool isTaskFound, bool isBarcodeValid)
    {
      var lifterId = "";
      var floor = "";
      var barcode = "";
      var task = new LifterTask(lifterId, floor, barcode, "", "", "");
      var isSaveCalled = false;
      var isTakenCalled = false;
      var command = Helper.UseService<ILifterCommandService>(mock => {
        mock.Setup(command => command.IsBarcodeValid(barcode))
          .Returns(isBarcodeValid);
        mock.Setup(command => command.GetBarcode(lifterId, floor))
          .Returns(barcode);
        mock.Setup(command => command.SetTaken(lifterId, floor))
          .Callback(() => isTakenCalled = true);
      });
      var repository = Helper.UseService<ILifterTaskRepository>(mock => {
        mock.Setup(repository => repository.FindFromRuntimeBarcode(barcode))
          .Returns(isTaskFound ? task : null);
        mock.Setup(repository => repository.SaveChanges())
          .Callback(() => isSaveCalled = true);
      });
      var service = new LifterService(command, repository, null);

      service.HandleTaken(lifterId, floor, barcode);

      Assert.IsTrue(isTakenCalled);
      Assert.AreEqual(isTaskFound && isBarcodeValid, isSaveCalled);
      Assert.AreEqual(isTaskFound && isBarcodeValid, task.Status == LifterTaskStatus.Taken);
    }
  }
}
