import { Plugin } from "vue";
import DataMapIterator from "./DataMapIterator.vue";

export default {
  install(app) {
    app.component("DataMapIterator", DataMapIterator);
  }
} as Plugin;
