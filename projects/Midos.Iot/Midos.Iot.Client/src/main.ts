/* eslint-disable indent */
import { MidOS } from "@midos/core";
import { VueUI } from "@midos/vue-ui";
import { AppIot } from "./app-iot";
import { IotHttpClient } from "./services/iot-http-client";
import { Confirm } from "./share/services/confirm";
import { Notify } from "./share/services/notify";

MidOS.create()
  .useUI(VueUI)
    .singleton(Confirm)
    .singleton(Notify)
  .addApp(AppIot)
    .addHttpClient(IotHttpClient)
  .build()
  .run();
