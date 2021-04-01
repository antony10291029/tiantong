import { HttpClient, injectable } from "@midos/core";
import { VueEnv, useService } from "@midos/vue-ui";

@injectable()
export class MidosCenterHttp extends HttpClient {
  public key = "MidosCenterHttp";

  public constructor(env: VueEnv) {
    super(env.IsStaging
      ? "http://172.16.2.74:4800"
      : env.IsProduction
        ? "http://172.16.2.64:4800"
        : "http://localhost:4800"
    );
  }
}

export function useMidosCenterHttp() {
  return useService(MidosCenterHttp);
}
