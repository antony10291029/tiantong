/* eslint-disable indent */
import { MidOS } from "@midos/core";
import { VueUI } from "@midos/vue-ui";
import { AppIot } from "./app-iot";
import { IotHttpClient } from "./services/iot-http-client";

MidOS.create()
  .useUI(VueUI)
  .addApp(AppIot)
    .addHttpClient(IotHttpClient)
  .build()
  .run();
