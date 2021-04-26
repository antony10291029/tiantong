/* eslint-disable indent */
import { MidOS } from "@midos/core";
import { VueUI, Confirm, Notify, HttpNotifyMiddleware, VueEnv } from "@midos/vue-ui";
import { NameiWcs } from "./app-namei-wcs";
import { NameiWcsHttp } from "./services/wcs-http";
import { RcsExtHttp } from "./services/rcs-ext-http";
import "@midos/vue-ui/style.sass";

MidOS.create()
  .singleton(VueEnv)
  .useUI(VueUI)
    .singleton(Confirm)
    .singleton(Notify)
  .addHttpMiddleware(HttpNotifyMiddleware)
  .addApp(NameiWcs)
    .addHttpClient(NameiWcsHttp)
    .addHttpClient(RcsExtHttp)
  .build()
  .run();
