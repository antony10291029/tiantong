import { injectable } from "@midos/core";
import { VueApp, VueUI } from "@midos/vue-ui";
import { RouteRecordRaw } from "vue-router";
import App from "./views/App/index.vue";
import Dashboard from "./views/Dashboard/index.vue";

@injectable()
export class ApiGateway extends VueApp {
  public constructor(private ui: VueUI) {
    super();
  }

  public key = "namei-api-gateway";

  public text = "API 网关";

  public icon = "icon-api-gateway icon-api-gateway-logo";

  public iconfont = "font_1745808_bcslrl0t1up";

  public route: RouteRecordRaw = {
    path: "/api-gateway",
    name: "ApiGateway",
    component: App,
    redirect: { name: "ApiGatewayDashboard" },
    children: [
      {
        path: "dashboard",
        name: "ApiGatewayDashboard",
        component: Dashboard
      }
    ]
  }
}
