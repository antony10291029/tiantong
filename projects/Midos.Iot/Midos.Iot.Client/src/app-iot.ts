import { injectable } from "@midos/core";
import { VueApp } from "@midos/vue-ui";
import { RouteRecordRaw } from "vue-router";
import App from "./views/Layout/index.vue";
import Plcs from "./views/Plcs/index.vue";
import PlcsCreate from "./views/Plcs.Create/index.vue";
import PlcsDashboard from "./views/Plcs.Dashboard/index.vue";
import PlcsPlc from "./views/Plcs.Plc/index.vue";
import PlcsPlcConfig from "./views/Plcs.Plc.Config/index.vue";
import PlcsPlcDashboard from "./views/Plcs.Plc.Dashboard/index.vue";
import PlcsPlcDebug from "./views/Plcs.Plc.Debug/index.vue";
import PlcsPlcLogs from "./views/Plcs.Plc.Logs/index.vue";
import PlcsPlcLogsHttpPusher from "./views/Plcs.Plc.Logs.HttpPusher/index.vue";
import PlcsPlcLogsHttpPusherErrors from "./views/Plcs.Plc.Logs.HttpPusherErrors/index.vue";
import PlcsPlcLogsStateLogs from "./views/Plcs.Plc.Logs.StateLogs/index.vue";
import PlcsPlcLogsStateErrors from "./views/Plcs.Plc.Logs.StateErrors/index.vue";
import PlcsPlcStatesCreate from "./views/Plcs.Plc.States.Create/index.vue";
import PlcsPlcStatesIndex from "./views/Plcs.Plc.States.Index/index.vue";
import PlcsPlcStatesState from "./views/Plcs.Plc.States.State/index.vue";
import PlcsPlcStatesStateDetail from "./views/Plcs.Plc.States.State.Detail/index.vue";
import PlcsPlcStatesStateHttpPushers from "./views/Plcs.Plc.States.State.HttpPushers/index.vue";

@injectable()
export class AppIot extends VueApp {
  public key = "app-iot";

  public text = "PLC 服务";

  public icon = "cpu";

  public route: RouteRecordRaw = {
    path: "/iot",
    name: "Iot",
    redirect: "/iot/plcs",
    component: App,
    children: [
      {
        path: "plcs",
        name: "IotPlcs",
        redirect: () => ({ name: "IotPlcsDashboard" }),
        component: Plcs,
        children: [
          {
            path: "",
            name: "IotPlcsDashboard",
            component: PlcsDashboard
          },
          {
            path: "create",
            name: "IotPlcsCreate",
            component: PlcsCreate
          },
          {
            path: ":plcId",
            name: "IotPlcsPlc",
            component: PlcsPlc,
            redirect: route => ({
              name: "IotPlcsPlcDashboard",
              params: { plcId: route.params.plcId }
            }),
            children: [
              {
                path: "dashboard",
                name: "IotPlcsPlcDashboard",
                component: PlcsPlcDashboard
              },
              {
                path: "debug",
                name: "IotPlcsPlcDebug",
                component: PlcsPlcDebug
              },
              {
                path: "config",
                name: "IotPlcsPlcConfig",
                component: PlcsPlcConfig
              },
              {
                path: "states",
                name: "IotPlcsPlcStatesIndex",
                component: PlcsPlcStatesIndex,
                children: [
                  {
                    path: "create",
                    name: "IotPlcsPlcStatesCreate",
                    component: PlcsPlcStatesCreate
                  },
                  {
                    path: ":stateId",
                    redirect: route => ({
                      name: "IotPlcsPlcStatesStateDetail",
                      params: {
                        plcId: route.params.plcId,
                        stateId: route.params.stateId,
                      }
                    }),
                    name: "IotPlcsPlcStatesState",
                    component: PlcsPlcStatesState,
                    children: [
                      {
                        path: "detail",
                        name: "IotPlcsPlcStatesStateDetail",
                        component: PlcsPlcStatesStateDetail
                      },
                      {
                        path: "http-posters",
                        name: "IotPlcsPlcStatesStateHttpPushers",
                        component: PlcsPlcStatesStateHttpPushers
                      },
                    ]
                  }
                ]
              },
              {
                path: "logs",
                name: "IotPlcsPlcLogs",
                redirect: route => ({
                  name: "IotPlcsPlcLogsStateLogs",
                  params: {
                    plcId: route.params.plcId
                  }
                }),
                component: PlcsPlcLogs,
                children: [
                  {
                    path: "state-logs",
                    name: "IotPlcsPlcLogsStateLogs",
                    component: PlcsPlcLogsStateLogs
                  },
                  {
                    path: "state-errors",
                    name: "IotPlcsPlcLogsStateErrors",
                    component: PlcsPlcLogsStateErrors
                  },
                  {
                    path: "http-pusher-logs",
                    name: "IotPlcsPlcLogsHttpPusher",
                    component: PlcsPlcLogsHttpPusher
                  },
                  {
                    path: "http-pusher-errors",
                    name: "IotPlcPlcsPlcLogsHttpPusherErrors",
                    component: PlcsPlcLogsHttpPusherErrors
                  },
                ]
              },
            ]
          },
        ]
      }
    ]
  }
}
