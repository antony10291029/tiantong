import { createRouter, createWebHistory } from "vue-router";
import WorkspaceLayout from "../layouts/Workspace/index.vue";
import WorkspaceIndex from "../pages/Workspace.Index/Index.vue";
import WorkspaceSettings from "../pages/Workspace.Settings/Settings.vue";
import PlcLayout from "../layouts/Plc/index.vue";

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: "/",
      name: "WorkspaceLayout",
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
      name: "Plc",
      component: PlcLayout,
      props: route => ({
        plcId: +route.params.plcId
      }),
      children: [

      ]
    }
  ]
});

export {
  router
};
