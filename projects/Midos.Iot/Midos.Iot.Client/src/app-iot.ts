import { injectable } from "@midos/core";
import { VueApp, VueUI } from "@midos/vue-ui";
import { RouteRecordRaw } from "vue-router";
import { plugin } from "./share";
import App from "./views/Layout/index.vue";
import Plcs from "./views/Plcs/index.vue";
import PlcsCreate from "./views/Plcs.Create/index.vue";
import PlcsDashboard from "./views/Plcs.Dashboard/index.vue";
import PlcsPlc from "./views/Plcs.Plc/index.vue";
import PlcsPlcConfig from "./views/Plcs.Plc.Config/index.vue";
import PlcsPlcDashboard from "./views/Plcs.Plc.Dashboard/index.vue";
import PlcsPlcDebug from "./views/Plcs.Plc.Debug/index.vue";

@injectable()
export class AppIot extends VueApp {
  public constructor(private ui: VueUI) {
    super();
  }

  public configure(): void {
    this.ui.app.use(plugin);
  }

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
                name: "PlcsPlcConfig",
                component: PlcsPlcConfig
              },
            ]
          },
        ]
      }
    ]
  }
}
