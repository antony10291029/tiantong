import { Plugin } from "vue";
import confirm from "./components/Confirm";
import directives from "./directives";
import notify from "./components/Notify";
import AsyncButton from "./components/AsyncButton.vue";
import AsyncLoader from "./components/AsyncLoader.vue";
import Checkbox from "./components/Checkbox.vue";
import Input from "./components/Input.vue";
import Radio from "./components/Radio.vue";
import Table from "./components/Table.vue";
import Textarea from "./components/Textarea.vue";
import Pagination from "./components/Pagination.vue";
import Switcher from "./components/Switcher.vue";

const plugin: Plugin = {
  install(app) {
    app.use(directives);
    app.component("AsyncButton", AsyncButton);
    app.component("AsyncLoader", AsyncLoader);
    app.component("Checkbox", Checkbox);
    app.component("Input", Input);
    app.component("Radio", Radio);
    app.component("Table", Table);
    app.component("Textarea", Textarea);
    app.component("Pagination", Pagination);
    app.component("Switcher", Switcher);
    app.use(confirm);
    app.use(notify);
  }
};

export {
  plugin
};
