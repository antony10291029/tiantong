import { injectable } from "@midos/core";
import { Repository } from "@midos/seed-work";
import { useService } from "@midos/vue-ui";
import { ApiGatewayHttp } from "../../services/api-gateway-http";
import { Endpoint } from "../entities";

@injectable()
export class EndpointRepository extends Repository<Endpoint> {
  public key = "endpoint-repository";

  public constructor(private http: ApiGatewayHttp) {
    super(http, "$endpoints");
  }
}

export function useEndpointRepository() {
  return useService(EndpointRepository);
}
