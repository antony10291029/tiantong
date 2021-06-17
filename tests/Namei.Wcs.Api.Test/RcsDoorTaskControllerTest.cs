using Microsoft.VisualStudio.TestTools.UnitTesting;
using Midos.Domain.Test;
using Moq;
using System;
using System.Linq;

namespace Namei.Wcs.Api.Test
{
  [TestClass]
  public class RcsDoorTaskControllerTest
  {
    private static RcsDoorTask _task = RcsDoorTask.From(
      uuid: "000001",
      doorId: "101"
    );

    private DomainContext _domain = new TestDomainContext();

    private IRcsService UseRcs(Action<Mock<IRcsService>> useRcs)
      => Helper.UseService<IRcsService>(useRcs);

    private IWcsDoorFactory UseDoors(Action<Mock<IWcsDoorFactory>> useDoors)
      => Helper.UseService<IWcsDoorFactory>(useDoors);

    private IWcsDoorService UseDoor(Action<Mock<IWcsDoorService>> useDoors)
      => Helper.UseService<IWcsDoorService>(useDoors);

    private RcsDoorEventController UseController(
      IWcsDoorFactory doors = null,
      IRcsService rcs = null
    ) {
      return new RcsDoorEventController(
        domain: _domain,
        doors: doors,
        rcs: rcs
      );
    }

    //

    [TestMethod]
    public void Test_Request_Create()
    {
      var controller = UseController();
      var param = RcsDoorEvent.From(
        uuid: "test.request.create",
        doorId: _task.DoorId
      );
      var task = RcsDoorTask.From(param);

      controller.HandleDoorTaskRequest(param);

      var data = _domain.Set<RcsDoorTask>().Find(task.Id);

      Assert.AreEqual(data.Id, task.Id);
      Assert.AreEqual(data.DoorId, task.DoorId);
      Assert.IsTrue(data.RequestedAt < DateTime.Now);
      Assert.IsTrue(data.RequestedAt.AddSeconds(1) > DateTime.Now);
      AssertHelper.HasEvent(RcsDoorEvent.Requested, param);
    }

    [TestMethod]
    public void Test_Request_Update()
    {
      var controller = UseController();
      var param = RcsDoorEvent.From(
        uuid: "test.requested.update",
        doorId: _task.DoorId
      );
      var task = RcsDoorTask.From(param);

      _domain.Add(task);
      _domain.SaveChanges();

      var requestedAt = task.RequestedAt;

      controller.HandleDoorTaskRequest(param);

      task = _domain.Set<RcsDoorTask>().Find(param.Uuid);

      Assert.IsTrue(requestedAt < task.RequestedAt);
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void Test_Requested(bool hasPassport)
    {
      var door = UseDoor(mock => mock
        .Setup(door => door.HasPassport)
        .Returns(hasPassport)
      );
      var doors = UseDoors(mock => mock
        .Setup(doors => doors.Get(_task.DoorId))
        .Returns(door)
      );
      var controller = UseController(doors);
      var param = RcsDoorEvent.From(
        uuid: $"test.requested.{hasPassport}",
        doorId: _task.DoorId
      );
      var task = RcsDoorTask.From(param);

      _domain.Add(task);
      _domain.SaveChanges();

      controller.HandleDoorTaskRequested(param);

      if (hasPassport) {
        AssertHelper.HasEvent(RcsDoorEvent.Enter, param);
      } else {
        AssertHelper.HasEvent(WcsDoorEvent.Open, WcsDoorEvent.From(param.DoorId));
      }
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void Test_Open(bool isOpened)
    {
      var isOpening = false;
      var door = UseDoor(mock => {
        mock.Setup(door => door.IsOpened)
          .Returns(isOpened);

        mock.Setup(door => door.Open())
          .Callback(() => isOpening = true);
      });
      var doors = UseDoors(mock => mock
        .Setup(doors => doors.Get(_task.DoorId))
        .Returns(door)
      );
      var controller = UseController(doors);
      var param = WcsDoorEvent.From(doorId: _task.DoorId);

      controller.HandleDoorOpen(WcsDoorEvent.From(
        doorId: _task.DoorId
      ));

      if (isOpened) {
        AssertHelper.HasEvent(WcsDoorEvent.Opened, param);
      }

      Assert.AreEqual(isOpening, true);
    }

    [TestMethod]
    public void Test_Opened()
    {
      var controller = UseController();
      var tasks = Enumerable
        .Range(1, 5)
        .Select(i => RcsDoorTask.From(
          uuid: $"test.opened.{i}",
          doorId: _task.DoorId
        ));

      _domain.AddRange(tasks);
      _domain.SaveChanges();

      controller.HandleDoorOpened(WcsDoorEvent.From(
        doorId: _task.DoorId
      ));

      foreach (var task in tasks) {
        AssertHelper.HasEvent(RcsDoorEvent.Enter, RcsDoorEvent.From(
          uuid: task.Id,
          doorId: _task.DoorId
        ));
      }
    }

    [TestMethod]
    public void Test_Enter()
    {
      var controller = UseController();
      var param = RcsDoorEvent.From(
        uuid: $"test.enter",
        doorId: _task.DoorId
      );
      var task = RcsDoorTask.From(param);

      _domain.Add(task);
      _domain.SaveChanges();

      controller.HandleDoorTaskEnter(param);

      task = _domain.Set<RcsDoorTask>().Find(task.Id);

      Assert.AreEqual(task.Status, RcsDoorTaskStatus.Entered);
      AssertHelper.HasEvent(RcsDoorEvent.Entered, param);
    }

    [TestMethod]
    public void Test_Entered()
    {
      var isNotified = false;
      var param = RcsDoorEvent.From(
        uuid: "test.entered",
        doorId: _task.DoorId
      );
      var rcs = UseRcs(mock => mock
        .Setup(rcs => rcs.NotifyDoorOpened(param.DoorId,param.Uuid))
        .Callback(() => isNotified = true)
      );
      var controller = UseController(
        rcs: rcs
      );

      controller.HandleDoorTaskEntered(param);

      Assert.AreEqual(isNotified, true);
    }

    [TestMethod]
    public void Test_Leave()
    {
      var param = RcsDoorEvent.From(
        uuid: "test.leave",
        doorId: _task.DoorId
      );
      var task = RcsDoorTask.From(param);
      var controller = UseController();

      _domain.Add(task);
      _domain.SaveChanges();

      controller.HandleDoorTaskLeave(param);

      var data = _domain.Set<RcsDoorTask>().Find(task.Id);

      Assert.AreEqual(data.Status, RcsDoorTaskStatus.Left);
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(10)]
    public void Test_Left(int count)
    {
      var doorId = $"test.left.{count}";
      var param = RcsDoorEvent.From(
        uuid: $"test.left.{count}",
        doorId: doorId
      );
      var task = RcsDoorTask.From(param);

      for (var i = 0; i < count; i++) {
        var data = RcsDoorTask.From(
          uuid: $"test.left.{count}.{i}",
          doorId: doorId
        );

        data.Enter();
        _domain.Add(data);
      }

      task.Leave();
      _domain.Add(task);
      _domain.SaveChanges();

      var controller = UseController();

      controller.HandleRcsDoorTaskLeft(param);

      var @event = WcsDoorEvent.From(task.DoorId);

      if (count == 0) {
        AssertHelper.HasEvent(WcsDoorEvent.Close, @event);
      } else {
        AssertHelper.HasNotEvent(WcsDoorEvent.Close, @event);
      }
    }

    [TestMethod]
    public void Test_Closing()
    {
      var isClosing = false;
      var param = WcsDoorEvent.From(_task.DoorId);
      var door = UseDoor(mock => mock
        .Setup(door => door.Close())
        .Callback(() => isClosing = true)
      );
      var doors = UseDoors(mock => mock
        .Setup(doors => doors.Get(param.DoorId))
        .Returns(door)
      );
      var controller = UseController(
        doors: doors
      );

      controller.HandleDoorClose(param);

      Assert.AreEqual(isClosing, true);
    }

    [TestMethod]
    [DataRow("1", "1")]
    [DataRow("4", "3")]
    public void Test_LifterTaskTaken(string floor, string lifterId)
    {
      var param = LifterTaskFinished.From(
        floor: floor,
        lifterId: lifterId
      );
      var doorId = DoorType.GetDoorIdFromLifter(
        floor: floor,
        lifterId: lifterId
      );
      var controller = UseController();

      controller.HandleLifterTaskTaken(param);

      var passport = _domain.Set<WcsDoorPassport>().Find(doorId);

      Assert.AreEqual(passport.Id, doorId);
      Assert.IsTrue(passport.ExpiredAt > DateTime.Now);
      Assert.IsTrue(passport.ExpiredAt.AddMinutes(-1) < DateTime.Now);
      AssertHelper.HasEvent(WcsDoorEvent.Opened, WcsDoorEvent.From(doorId));
    }
  }
}
