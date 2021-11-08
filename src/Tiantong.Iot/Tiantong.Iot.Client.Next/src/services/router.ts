import { createRouter, createWebHistory } from "vue-router";
import WorkspaceLayout from "../pages/Workspace/index.vue";
import WorkspaceIndex from "../pages/Workspace.Index/Index.vue";
import WorkspaceSettings from "../pages/Workspace.Settings/Settings.vue";
import PlcLayout from "../pages/Plc/index.vue";
import PlcSettings from "../pages/Plc.Settings/Settings.vue";
import PlcDebug from "../pages/Plc.Debug/index.vue";
import PlcStates from "../pages/Plc.States/index.vue";

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: "/",
      name: "Workspace.Layout",
      component: WorkspaceLayout,
      children: [
        {
          path: "",
          name: "Workspace.Index",
          component: WorkspaceIndex
        },
        {
          path: "/workspace/settings",
          name: "Workspace.Settings",
          component: WorkspaceSettings
        }
      ]
    },
    {
      path: "/plcs/:plcId",
      name: "Plc.Layout",
      component: PlcLayout,
      props: route => ({
        plcId: +route.params.plcId
      }),
      children: [
        {
          path: "",
          name: "PlcSettings",
          component: PlcSettings,
        },
        {
          path: "debug",
          name: "PlcDebug",
          component: PlcDebug
        },
        {
          path: "states",
          name: "PlcStates",
          component: PlcStates
        }
      ]
    }
  ]
});

export {
  router
};
