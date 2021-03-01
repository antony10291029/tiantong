import { HttpClient, injectable } from "@midos/core";
import { useService, VueEnv } from "@midos/vue-ui";

@injectable()
export class IotHttpClient extends HttpClient {
  public key = "IotHttpClient";

  public constructor(env: VueEnv) {
    super(env.IsStaging
      ? "http://172.16.2.74:5101"
      : env.IsProduction
      ? "http://172.16.2.65:51001"
      : "http://localhost:5101"
    );
  }
}

export function useIotHttp() {
  return useService(IotHttpClient);
}
