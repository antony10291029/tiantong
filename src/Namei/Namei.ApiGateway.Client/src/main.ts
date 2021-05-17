/* eslint-disable indent */
import { MidOS } from "@midos/core";
import { DomainSeedWork } from "@midos/seed-work";
import { VueUI, Confirm, Notify, HttpNotifyMiddleware, VueEnv } from "@midos/vue-ui";
import { ApiGateway } from "./app-api-gateway";
import { ApiGatewayHttp } from "./services/api-gateway-http";
import { EndpointRepository } from "./domain/repositories/endpoint-repository";
import { RouteRepository } from "./domain/repositories/route-repository";
import "@midos/vue-ui/style.sass";

MidOS.create()
  .singleton(VueEnv)
  .useUI(VueUI)
    .singleton(Confirm)
    .singleton(Notify)
  .useSeedWork(DomainSeedWork)
    .addHttpMiddleware(HttpNotifyMiddleware)
  .addApp(ApiGateway)
    .addHttpClient(ApiGatewayHttp)
    .singleton(EndpointRepository)
    .singleton(RouteRepository)
  .build()
  .run();
