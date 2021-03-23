// using DotNetCore.CAP;
// using Microsoft.AspNetCore.Mvc;

// namespace Namei.Wcs.Api
// {
//   public class DoorEventController: BaseController
//   {
//     const string Group = "command";

//     private DoorServiceManager _doors;

//     public DoorEventController(DoorServiceManager doors)
//     {
//       _doors = doors;
//     }

//     [CapSubscribe(DoorOpenedEvent.Message, Group = Group)]
//     public void HandleDoorOpened(DoorOpenedEvent param)
//     {
//       _doors.Get(param.DoorId).OnOpened();
//     }

//     [CapSubscribe(DoorClosedEvent.Message, Group = Group)]
//     public void HandleDoorClosed(DoorClosedEvent param)
//     {
//       _doors.Get(param.DoorId).OnClosed();
//     }

//     // 放货完成
//     [CapSubscribe(LifterTaskImported.Message, Group = Group)]
//     public void HandleLifterTaskImported(LifterTaskImported param)
//     {
//       var doorId = CrashDoor.GetDoorIdFromLifter(param.Floor, param.LifterId);

//       _doors.Get(doorId).Open();
//     }

//     // 取货完成
//     [CapSubscribe(LifterTaskTaken.Message, Group = Group)]
//     public void HandleLifterTaskTaken(LifterTaskTaken param)
//     {
//       var doorId = CrashDoor.GetDoorIdFromLifter(param.Floor, param.LifterId);

//       _doors.Get(doorId).Open();
//     }
//   }
// }
