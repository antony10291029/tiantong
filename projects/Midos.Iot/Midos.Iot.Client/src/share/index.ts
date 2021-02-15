import { Plugin } from "vue";
import AsyncButton from "./components/AsyncButton.vue";
import AsyncLoader from "./components/AsyncLoader.vue";
import Checkbox from "./components/Checkbox.vue";
import Table from "./components/Table.vue";
import Pagination from "./components/Pagination.vue";
import directives from "./directives";

const plugin: Plugin = {
  install(app) {
    app.component("AsyncButton", AsyncButton);
    app.component("AsyncLoader", AsyncLoader);
    app.component("Checkbox", Checkbox);
    app.component("Table", Table);
    app.component("Pagination", Pagination);
    app.use(directives);
  }
};

export {
  plugin
};
