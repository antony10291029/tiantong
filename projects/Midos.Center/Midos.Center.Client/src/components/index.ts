import { Plugin } from "vue";
import SearchField from "./SearchField.vue";
import TimeWrapper from "./TimeWrapper.vue";

const components = {
  install(app) {
    app.component("SearchField", SearchField);
    app.component("TimeWrapper", TimeWrapper);
  }
} as Plugin;

export {
  components
};
