import { injectable } from "@midos/core";
import { VueApp, VueUI } from "@midos/vue-ui";
import { RouteRecordRaw } from "vue-router";
import plugin from "./plugin";
import RouteTab from "./components/RouteTab.vue";
import App from "./views/App/index.vue";
import DevicesDashboard from "./views/Devices.Dashboard/index.vue";
import DevicesLifterTasks from "./views/Devices.Lifter.Tasks/index.vue";
import System from "./views/System/index.vue";
import LifterLogs from "./views/Lifters.Logs/index.vue";
import LifterTasks from "./views/Lifters.Tasks/index.vue";
import DoorLogs from "./views/Doors.Logs/index.vue";
import DoorTasks from "./views/Doors.Tasks/index.vue";
import DoorCommands from "./views/Doors.Commands/index.vue";
import RcsUnbind from "./views/Rcs.Unbind/index.vue";
import RcsTasks from "./views/Rcs.Tasks/index.vue";
import RcsNotifyTask from "./views/Rcs.NotifyTask/index.vue";
import Logs from "./views/Logs/index.vue";
import WmsPickTicketTasks from "./views/Wms.PickTicketTasks/index.vue";
import RcsAgcTasks from "./views/Rcs.Agc.Tasks/index.vue";
import RcsAgcTaskCreate from "./views/Rcs.Agc.Tasks.Create/index.vue";
import RcsAgcTaskLogs from "./views/Rcs.Agc.Tasks.Logs/index.vue";

@injectable()
export class NameiWcs extends VueApp {
  public constructor(private ui: VueUI) {
    super();
  }

  public configure() {
    this.ui.app.use(plugin);
  }

  public key = "namei-wcs";

  public text = "WCS";

  public icon = "icon-namei-wcs icon-namei-wcs-logo";

  public iconfont = "font_1966999_2h6nrreb7vd";

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
        redirect: { name: "NameiWcsLifterTasks" },
        component: RouteTab,
        props: () => ({
          tabs: [
            { text: "任务列表", route: "NameiWcsLifterTasks" },
            { text: "运行日志", route: "NameiWcsLifterLogs" },
          ]
        }),
        children: [
          {
            path: "logs",
            name: "NameiWcsLifterLogs",
            component: LifterLogs
          },
          {
            path: "tasks",
            name: "NameiWcsLifterTasks",
            component: LifterTasks
          }
        ]
      },
      {
        path: "doors",
        name: "NameiWcsDoors",
        redirect: { name: "NameiWcsDoorTasks" },
        component: RouteTab,
        props: () => ({
          tabs: [
            { text: "任务列表", route: "NameiWcsDoorTasks" },
            { text: "运行日志", route: "NameiWcsDoorsLogs" },
            { text: "控制指令", route: "NameiWcsDoorsCommands" },
          ]
        }),
        children: [
          {
            path: "tasks",
            name: "NameiWcsDoorTasks",
            component: DoorTasks
          },
          {
            path: "logs",
            name: "NameiWcsDoorsLogs",
            component: DoorLogs
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
      },
      {
        path: "/wms/pick-ticket-tasks",
        name: "NameiWcsWmsPickTicketTasks",
        component: WmsPickTicketTasks
      },
      {
        path: "/rcs/agc-tasks",
        name: "NameiWcsRcsAgcTasks",
        redirect: { name: "NameiWcsRcsAgcTasksSearch" },
        component: RouteTab,
        props: () => ({
          tabs: [
            { text: "任务列表", route: "NameiWcsRcsAgcTasksSearch" },
            { text: "运行日志", route: "NameiWcsRcsAgcTasksLogs" },
          ]
        }),
        children: [
          {
            path: "search",
            name: "NameiWcsRcsAgcTasksSearch",
            component: RcsAgcTasks,
            children: [
              {
                path: "create",
                name: "NameiWcsRcsAgcTasksCreate",
                component: RcsAgcTaskCreate
              }
            ]
          },
          {
            path: "logs",
            name: "NameiWcsRcsAgcTasksLogs",
            component: RcsAgcTaskLogs
          }
        ]
      },
    ]
  };
}
