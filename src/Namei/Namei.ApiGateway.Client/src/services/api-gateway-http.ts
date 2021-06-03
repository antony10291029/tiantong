import { HttpClient, injectable } from "@midos/core";
import { useService, VueEnv } from "@midos/vue-ui";

@injectable()
export class ApiGatewayHttp extends HttpClient {
  public key = "ApiGatewayHttp";

  public constructor(private env: VueEnv) {
    super(env.IsStaging
      ? "http://172.16.2.62"
      : env.IsProduction
        ? "http://172.16.2.62"
        : "http://localhost:5200");
  }
}

export function UseApiGatewayHttp() {
  return useService(ApiGatewayHttp);
}
