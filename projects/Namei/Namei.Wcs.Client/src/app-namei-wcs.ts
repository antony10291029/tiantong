import { injectable } from "@midos/core";
import { VueApp, VueUI } from "@midos/vue-ui";
import { RouteRecordRaw } from "vue-router";
import RouteTab from "./components/RouteTab.vue";
import App from "./views/App/index.vue";
import DevicesDashboard from "./views/Devices.Dashboard/index.vue";
import DevicesLifterTasks from "./views/Devices.Lifter.Tasks/index.vue";
import System from "./views/System/index.vue";
import LifterLogs from "./views/Lifters.Logs/index.vue";
import DoorsStates from "./views/Doors.Logs/index.vue";
import DoorCommands from "./views/Doors.Commands/index.vue";
import RcsUnbind from "./views/Rcs.Unbind/index.vue";
import RcsTasks from "./views/Rcs.Tasks/index.vue";
import RcsNotifyTask from "./views/Rcs.NotifyTask/index.vue";
import Logs from "./views/Logs/index.vue";

@injectable()
export class NameiWcs extends VueApp {
  public constructor(private ui: VueUI) {
    super();
  }

  public key = "namei-wcs";

  public text = "WCS";

  public icon = "icon-namei-wcs icon-namei-wcs-logo";

  public iconfont = "font_1966999_p96j2xvf5bl";

  public route: RouteRecordRaw = {
    path: "/namei-wcs",
    name: "NameiWcs",
    redirect: { name: "NameiWcsDevicesDashboard" },
    component: App,
    children: [
      {
        path: "logs",
        name: "NameiWcsLogs",
        component: Logs
      },
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
      {
        path: "system",
        name: "NameiWcsSystem",
        component: System
      },
      {
        path: "lifters",
        name: "NameiWcsLifters",
        redirect: { name: "NameiWcsLifterLogs" },
        component: RouteTab,
        props: () => ({
          tabs: [
            { text: "运行日志", route: "NameiWcsLifterLogs" },
          ]
        }),
        children: [
          {
            path: "logs",
            name: "NameiWcsLifterLogs",
            component: LifterLogs
          },
        ]
      },
      {
        path: "doors",
        name: "NameiWcsDoors",
        redirect: { name: "NameiWcsDoorsLogs" },
        component: RouteTab,
        props: () => ({
          tabs: [
            { text: "运行日志", route: "NameiWcsDoorsLogs" },
            { text: "控制指令", route: "NameiWcsDoorsCommands" },
          ]
        }),
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
        redirect: { name: "NameiWcsNotifyTask" },
        props: () => ({
          tabs: [
            { text: "任务通知", route: "NameiWcsNotifyTask" },
            { text: "地图绑定", route: "NameiWcsRcsUnbind" },
            { text: "任务调度", route: "NameiWcsRcsTasks" },
          ],
        }),
        component: RouteTab,
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
          },
          {
            path: "notify-task",
            name: "NameiWcsNotifyTask",
            component: RcsNotifyTask
          }
        ]
      }
    ]
  };
}
