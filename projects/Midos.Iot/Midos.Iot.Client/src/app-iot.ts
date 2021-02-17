import { injectable } from "@midos/core";
import { VueApp, VueUI } from "@midos/vue-ui";
import { RouteRecordRaw } from "vue-router";
import { plugin } from "./share";
import App from "./views/Layout/index.vue";
import Plcs from "./views/Plcs/index.vue";
import PlcsPlc from "./views/Plcs.Plc/index.vue";
import PlcsPlcDashboard from "./views/Plcs.Plc.Dashboard/index.vue";
import PlcsCreate from "./views/Plcs.Create/index.vue";

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
        name: "Plcs",
        component: Plcs,
        children: [
          {
            path: "create",
            name: "PlcsCreate",
            component: PlcsCreate
          },
          {
            path: ":plcId",
            name: "PlcsPlc",
            component: PlcsPlc,
            redirect: (route) => `/iot/plcs/${route.params.plcId}/dashboard`,
            children: [
              {
                path: "dashboard",
                name: "PlcsPlcDashboard",
                component: PlcsPlcDashboard
              },
            ]
          },
        ]
      }
    ]
  }
}
