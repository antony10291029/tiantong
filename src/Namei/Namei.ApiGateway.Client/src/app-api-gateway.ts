import { injectable } from "@midos/core";
import { VueApp, VueUI } from "@midos/vue-ui";
import { RouteRecordRaw } from "vue-router";
import App from "./views/App/index.vue";
import Dashboard from "./views/Dashboard/index.vue";
import Endpoints from "./views/Endpoints/index.vue";
import EndpointsEndpoint from "./views/Endpoints.Endpoint/index.vue";
import EndpointsCreate from "./views/Endpoints.Create/index.vue";
import Routes from "./views/Routes/index.vue";
import Settings from "./views/Settings/index.vue";

@injectable()
export class ApiGateway extends VueApp {
  public constructor(private ui: VueUI) {
    super();
  }

  public key = "namei-api-gateway";

  public text = "API 网关";

  public icon = "icon-api-gateway icon-api-gateway-logo";

  public iconfont = "font_1745808_xmkvwmtiay";

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
      },
      {
        path: "endpoints",
        name: "ApiGatewayEndpoints",
        component: Endpoints,
        children: [
          {
            path: "create",
            name: "ApiGatewayEndpointsCreate",
            component: EndpointsCreate
          },
          {
            path: ":endpointId",
            name: "ApiGatewayEndpointsEndpoint",
            props: route => ({ id: +route.params.endpointId }),
            component: EndpointsEndpoint
          }
        ]
      },
      {
        path: "routes",
        name: "ApiGatewayRoutes",
        component: Routes
      },
      {
        path: "settings",
        name: "ApiGatewaySettings",
        component: Settings
      }
    ]
  }
}
