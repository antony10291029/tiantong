import { Plugin } from "vue";
import SearchField from "./SearchField.vue";

const components = {
  install(app) {
    app.component("SearchField", SearchField);
  }
} as Plugin;

export {
  components
};
