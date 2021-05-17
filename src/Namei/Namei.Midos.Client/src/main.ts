/* eslint-disable indent */
import { MidOS } from "@midos/core";
import { DomainSeedWork } from "@midos/seed-work";
import { VueUI, Confirm, Notify, HttpNotifyMiddleware, Config, VueEnv } from "@midos/vue-ui";
import { MidosCenter, MidosCenterHttp } from "@midos/app-midos-center";
import { NameiWcs, NameiWcsHttp } from "@midos/app-namei-wcs";
import { AppIot, IotHttpClient } from "@midos/app-iot";
import { ApiGateway, ApiGatewayHttp, RouteRepository, EndpointRepository } from "@midos/app-api-gateway";
import "@midos/vue-ui/style.sass";

MidOS.create()
  .singleton(Config)
  .useUI(VueUI)
    .singleton(Confirm)
    .singleton(Notify)
    .singleton(VueEnv)
  .useSeedWork(DomainSeedWork)
  .addHttpMiddleware(HttpNotifyMiddleware)
  .addApp(NameiWcs)
    .addHttpClient(NameiWcsHttp)
  .addApp(ApiGateway)
    .addHttpClient(ApiGatewayHttp)
  .addApp(AppIot)
    .addHttpClient(IotHttpClient)
  .addApp(MidosCenter)
    .addHttpClient(MidosCenterHttp)
    .singleton(RouteRepository)
    .singleton(EndpointRepository)
  .build()
  .run();
