import { injectable } from "@midos/core";
import { Repository } from "@midos/seed-work";
import { useService } from "@midos/vue-ui";
import { ApiGatewayHttp } from "../../services/api-gateway-http";
import { Route } from "../entities";

@injectable()
export class RouteRepository extends Repository<Route> {
  public key = "route-repository";

  public constructor(private http: ApiGatewayHttp) {
    super(http, "$routes");
  }
}

export function useRouteRepository() {
  return useService(RouteRepository);
}
