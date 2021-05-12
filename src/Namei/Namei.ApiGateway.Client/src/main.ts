/* eslint-disable indent */
import { MidOS } from "@midos/core";
import { VueUI, Confirm, Notify, HttpNotifyMiddleware, VueEnv } from "@midos/vue-ui";
import { ApiGateway } from "./app-api-gateway";
import "@midos/vue-ui/style.sass";

MidOS.create()
  .singleton(VueEnv)
  .useUI(VueUI)
    .singleton(Confirm)
    .singleton(Notify)
  .addHttpMiddleware(HttpNotifyMiddleware)
  .addApp(ApiGateway)
  .build()
  .run();
