import { injectable } from "@midos/core";
import { VueApp } from "@midos/vue-ui";
import { RouteRecordRaw } from "vue-router";
import App from "./views/App/index.vue";
import Apps from "./views/Midos.Apps/index.vue";
import AppsApp from "./views/Midos.Apps.App/index.vue";
import AppsCreate from "./views/Midos.Apps.Create/index.vue";
import Configs from "./views/Configs/index.vue";
import ConfigsCreate from "./views/Configs.Create/index.vue";
import ConfigsConfig from "./views/Configs.Config/index.vue";

@injectable()
export class MidosCenter extends VueApp {
  public key = "midos-center";

  public text = "系统管理";

  public icon = "icon-midos-center icon-midos-center-logo";

  public iconfont = "font_2390325_ufprs0zt6uk";

  public route: RouteRecordRaw = {
    path: "/midos-center",
    name: "MidosCenter",
    redirect: { name: "MidosCenterApps" },
    component: App,
    children: [
      {
        path: "apps",
        name: "MidosCenterApps",
        component: Apps,
        children: [
          {
            path: "create",
            name: "MidosCenterAppsCreate",
            component: AppsCreate,
          },
          {
            path: ":id",
            name: "MidosCenterAppsApp",
            component: AppsApp,
            props: route => ({ id: +route.params.id })
          }
        ]
      },
      {
        path: "configs",
        name: "MidosCenterConfigs",
        component: Configs,
        children: [
          {
            path: "create",
            name: "MidosCenterConfigsCreate",
            component: ConfigsCreate
          },
          {
            path: ":configKey",
            name: "MidosCenterConfigsConfig",
            component: ConfigsConfig,
            props: route => route.params
          }
        ]
      }
    ]
  }
}
