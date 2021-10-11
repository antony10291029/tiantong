import { Plugin } from "vue";
import notify from "./Notify";
import confirm from "./Confirm";

export default {
  install(app) {
    app.use(notify);
    app.use(confirm);
  }
} as Plugin;
