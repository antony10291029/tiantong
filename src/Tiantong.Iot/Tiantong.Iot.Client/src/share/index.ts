import { Plugin } from "vue";
import AsyncLoader from "./components/AsyncLoader.vue";

const plugin: Plugin = {
  install(app) {
    app.component("AsyncLoader", AsyncLoader);
  }
};

export {
  plugin
};
