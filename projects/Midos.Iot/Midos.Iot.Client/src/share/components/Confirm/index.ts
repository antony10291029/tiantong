import { Plugin, createApp } from "vue";
import Component from "./Component.vue";
import directives from "../../directives";

export { IConfirm } from "./interface";

export default {
  install (app) {
    const dom = document.createElement("div");
    const vm = createApp(Component).use(directives).mount(dom);

    document.body.appendChild(dom);
    app.config.globalProperties.$confirm = vm;
  }
} as Plugin;
