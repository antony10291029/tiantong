import { createApp } from "vue";
import { router } from "./services/router";
import App from "./App.vue";
import Confirm from "./shared/Confirm";
import Notify from "./shared/Notify";

createApp(App)
  .use(router)
  .use(Confirm)
  .use(Notify)
  .mount("#app");
