/* eslint-disable indent */
import { MidOS } from "@midos/core";
import { VueUI, Confirm, Notify, HttpNotifyMiddleware, Config } from "@midos/vue-ui";
import { NameiWcs, NameiWcsHttp } from "@midos/app-namei-wcs";
import { AppIot, IotHttpClient } from "@midos/app-iot";
import "@midos/vue-ui/style.sass";

MidOS.create()
  .singleton(Config)
  .useUI(VueUI)
    .singleton(Confirm)
    .singleton(Notify)
  .addHttpMiddleware(HttpNotifyMiddleware)
  .addApp(NameiWcs)
    .addHttpClient(NameiWcsHttp)
  .addApp(AppIot)
    .addHttpClient(IotHttpClient)
  .build()
  .run();
