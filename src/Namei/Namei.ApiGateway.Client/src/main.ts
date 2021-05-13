/* eslint-disable indent */
import { MidOS } from "@midos/core";
import { VueUI, Confirm, Notify, HttpNotifyMiddleware, VueEnv } from "@midos/vue-ui";
import { ApiGateway } from "./app-api-gateway";
import "@midos/vue-ui/style.sass";
import { ApiGatewayHttp } from "./services/api-gateway-http";

MidOS.create()
  .singleton(VueEnv)
  .useUI(VueUI)
    .singleton(Confirm)
    .singleton(Notify)
  .addHttpMiddleware(HttpNotifyMiddleware)
    .addHttpClient(ApiGatewayHttp)
  .addApp(ApiGateway)
  .build()
  .run();
