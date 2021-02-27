import { HttpClient, injectable } from "@midos/core";
import { VueEnv, useService } from "@midos/vue-ui";

@injectable()
export class MidosCenterHttp extends HttpClient {
  public key = "MidosCenterHttp";

  public constructor(env: VueEnv) {
    super(env.getValue("MIDOS_URL"));
  }
}

export function useMidosCenterHttp() {
  return useService(MidosCenterHttp);
}
