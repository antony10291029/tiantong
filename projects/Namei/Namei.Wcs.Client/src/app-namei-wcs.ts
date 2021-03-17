import { injectable } from "@midos/core";
import { VueApp, VueUI } from "@midos/vue-ui";
import { RouteRecordRaw } from "vue-router";
import App from "./views/App/index.vue";
import Devices from "./views/Devices/index.vue";
import DevicesDashboard from "./views/Devices.Dashboard/index.vue";
import DevicesLifterTasks from "./views/Devices.Lifter.Tasks/index.vue";
import System from "./views/System/index.vue";
import Lifters from "./views/Lifters/index.vue";
import LifterLogs from "./views/Lifters.Logs/index.vue";
import LifterCommands from "./views/Lifters.Commands/index.vue";
import Doors from "./views/Doors/index.vue";
import DoorsStates from "./views/Doors.Logs/index.vue";
import DoorCommands from "./views/Doors.Commands/index.vue";
import Rcs from "./views/Rcs/index.vue";
import RcsUnbind from "./views/Rcs.Unbind/index.vue";
import RcsTasks from "./views/Rcs.Tasks/index.vue";
import Logs from "./views/Logs/index.vue";

@injectable()
export class NameiWcs extends VueApp {
  public constructor(private ui: VueUI) {
    super();
  }

  public key = "namei-wcs";

  public text = "WCS";

  public icon = "icon-namei-wcs icon-namei-wcs-logo";

  public iconfont = "font_1966999_e6niplqg6ug";

  public route: RouteRecordRaw = {
    path: "/namei-wcs",
    name: "NameiWcs",
    redirect: { name: "NameiWcsDevices" },
    component: App,
    children: [
      {
        path: "logs",
        name: "NameiWcsLogs",
        component: Logs
      },
      {
        path: "devices",
        name: "NameiWcsDevices",
        redirect: { name: "NameiWcsDevicesDashboard" },
        component: Devices,
        children: [
          {
            path: "dashboard",
            name: "NameiWcsDevicesDashboard",
            component: DevicesDashboard,
          },
          {
            path: "lifter-tasks",
            name: "NameiWcsDevicesLifterTasks",
            component: DevicesLifterTasks,
          },
        ]
      },
      {
        path: "system",
        name: "NameiWcsSystem",
        component: System
      },
      {
        path: "lifters",
        name: "NameiWcsLifters",
        redirect: { name: "NameiWcsLifterLogs" },
        component: Lifters,
        children: [
          {
            path: "logs",
            name: "NameiWcsLifterLogs",
            component: LifterLogs
          },
          {
            path: "commands",
            name: "NameiWcsLifterCommands",
            component: LifterCommands
          }
        ]
      },
      {
        path: "doors",
        name: "NameiWcsDoors",
        redirect: { name: "NameiWcsDoorsLogs" },
        component: Doors,
        children: [
          {
            path: "logs",
            name: "NameiWcsDoorsLogs",
            component: DoorsStates
          },
          {
            path: "commands",
            name: "NameiWcsDoorsCommands",
            component: DoorCommands
          }
        ]
      },
      {
        path: "rcs",
        name: "NameiWcsRcs",
        redirect: { name: "NameiWcsRcsUnbind" },
        component: Rcs,
        children: [
          {
            path: "rcs-unbind",
            name: "NameiWcsRcsUnbind",
            component: RcsUnbind
          },
          {
            path: "tasks",
            name: "NameiWcsRcsTasks",
            component: RcsTasks
          }
        ]
      }
    ]
  };
}
