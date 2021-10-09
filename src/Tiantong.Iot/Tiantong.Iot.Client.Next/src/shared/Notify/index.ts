import { createApp, Plugin, App } from "vue";
import Component from "./Component.vue";
import type { Notify } from "./interface";

// eslint-disable-next-line import/no-mutable-exports
let notify: Notify;

const plugin: Plugin = {
  install(app: App) {
    const el = document.createElement("div");

    notify = createApp(Component).mount(el) as any;
    document.body.appendChild(el);
  }
};

export {
  notify
};

export default plugin;
