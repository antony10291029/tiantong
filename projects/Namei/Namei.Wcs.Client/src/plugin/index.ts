import { Plugin } from "vue";
import DataSetIterator from "./DataSetIterator.vue";

export default {
  install(app) {
    app.component("DataSetIterator", DataSetIterator);
  }
} as Plugin;
