/* eslint-disable indent */
import { MidOS } from "@midos/core";
import { VueUI, Confirm, Notify, HttpNotifyMiddleware, Config } from "@midos/vue-ui";
import { AppIot } from "./app-iot";
import { IotHttpClient } from "./services/iot-http-client";
import "@midos/vue-ui/style.sass";

MidOS.create()
  .singleton(Config)
  .useUI(VueUI)
    .singleton(Confirm)
    .singleton(Notify)
  .addHttpMiddleware(HttpNotifyMiddleware)
  .addApp(AppIot)
    .addHttpClient(IotHttpClient)
  .build()
  .run();
