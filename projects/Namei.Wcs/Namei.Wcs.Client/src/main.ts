/* eslint-disable indent */
import { MidOS } from "@midos/core";
import { VueUI, Confirm, Notify, HttpNotifyMiddleware, Config } from "@midos/vue-ui";
import { NameiWcs } from "./app-namei-wcs";
import { NameiWcsHttp } from "./services/wcs-http";
import "@midos/vue-ui/style.sass";

MidOS.create()
  .singleton(Config)
  .useUI(VueUI)
    .singleton(Confirm)
    .singleton(Notify)
  .addHttpMiddleware(HttpNotifyMiddleware)
  .addApp(NameiWcs)
    .addHttpClient(NameiWcsHttp)
  .build()
  .run();
