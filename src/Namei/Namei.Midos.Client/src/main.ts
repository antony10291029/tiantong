/* eslint-disable indent */
import { MidOS } from "@midos/core";
import { DomainSeedWork } from "@midos/seed-work";
import { VueUI, Confirm, Notify, HttpNotifyMiddleware, Config, VueEnv } from "@midos/vue-ui";
import { NameiWcs, NameiWcsHttp, RcsExtHttp } from "@midos/app-namei-wcs";
import { AppIot, IotHttpClient } from "@midos/app-iot";
import { ApiGateway, ApiGatewayHttp, RouteRepository, EndpointRepository } from "@midos/app-api-gateway";
import "@midos/vue-ui/style.sass";

MidOS.create()
  .singleton(VueEnv)
  .useUI(VueUI)
    .singleton(Confirm)
    .singleton(Notify)
  .useSeedWork(DomainSeedWork)
  .addHttpMiddleware(HttpNotifyMiddleware)
  .addApp(NameiWcs)
    .addHttpClient(NameiWcsHttp)
    .addHttpClient(RcsExtHttp)
  .addApp(ApiGateway)
    .addHttpClient(ApiGatewayHttp)
    .singleton(RouteRepository)
    .singleton(EndpointRepository)
  .addApp(AppIot)
    .addHttpClient(IotHttpClient)
  .build()
  .run();
