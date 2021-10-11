import { createApp, Plugin, App, getCurrentInstance } from "vue";
import { Options } from "./interface";
import Component from "./Component.vue";

const plugin: Plugin = {
  install(app: App, el) {
    if (!el) {
      el = document.createElement("div");
      document.body.appendChild(el);
    }

    const vm = createApp(Component).mount(el);
    app.config.globalProperties.$confirm = vm;
  }
};

export function useConfirm(): (options: Options) => void {
  const app = getCurrentInstance();

  return app?.appContext.config.globalProperties.$confirm.open;
}

export default plugin;
