/* eslint-disable indent */
import { MidOS } from "@midos/core";
import { VueUI, Confirm, Notify, HttpNotifyMiddleware, VueEnv } from "@midos/vue-ui";
import { DomainSeedWork } from "@midos/seed-work";
import { AppIot } from "./app-iot";
import { IotHttpClient } from "./services/iot-http-client";
import "@midos/vue-ui/style.sass";

MidOS.create()
  .singleton(VueEnv)
  .useUI(VueUI)
    .singleton(Confirm)
    .singleton(Notify)
  .useSeedWork(DomainSeedWork)
  .addHttpMiddleware(HttpNotifyMiddleware)
  .addApp(AppIot)
    .addHttpClient(IotHttpClient)
  .build()
  .run();
