import { Plugin, createApp } from "vue";
import Component from "./NotificationContainer.vue";
import directives from "../../directives";

export default {
  install (app) {
    const dom = document.createElement("div");
    const vm = createApp(Component).use(directives).mount(dom);

    document.body.appendChild(dom);
    app.config.globalProperties.$notify = vm;
  }
} as Plugin;
