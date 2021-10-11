import { createApp } from "vue";
import { router } from "./services/router";
import App from "./App.vue";
import components from "./shared/components";

createApp(App)
  .use(router)
  .use(components)
  .mount("#app");
