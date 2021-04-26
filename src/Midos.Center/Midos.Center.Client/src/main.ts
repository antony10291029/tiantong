/* eslint-disable indent */
import { MidOS } from "@midos/core";
import { VueUI, Confirm, Notify, HttpNotifyMiddleware, Config } from "@midos/vue-ui";
import { MidosCenter } from "./app-midos-center";
import { MidosCenterHttp } from "./services/midos-center-http";
import "@midos/vue-ui/style.sass";

MidOS.create()
  .singleton(Config)
  .useUI(VueUI)
    .singleton(Confirm)
    .singleton(Notify)
  .addHttpMiddleware(HttpNotifyMiddleware)
  .addApp(MidosCenter)
    .addHttpClient(MidosCenterHttp)
  .build()
  .run();
