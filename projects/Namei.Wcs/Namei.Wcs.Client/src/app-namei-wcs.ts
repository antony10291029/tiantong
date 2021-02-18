import { injectable } from "@midos/core";
import { VueApp } from "@midos/vue-ui";
import { RouteRecordRaw } from "vue-router";
import Layout from "./views/Layout/index.vue";
import Devices from "./views/Devices/index.vue";
import System from "./views/System/index.vue";

@injectable()
export class NameiWcs extends VueApp {
  public key = "namei-wcs";

  public text = "WCS";

  public icon = "namei-wcs-logo";

  public iconfont = "font_1966999_xzvdh8z5d8i";

  public route: RouteRecordRaw = {
    path: "/namei-wcs",
    name: "NameiWcs",
    component: Layout,
    children: [
      {
        path: "/devices",
        name: "NameiWcsDevices",
        component: Devices,
      },
      {
        path: "system",
        name: "NameiWcsSystem",
        component: System
      },
    ]
  }
}
