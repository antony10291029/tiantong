import { HttpClient, injectable } from "@midos/core";
import { useService, VueEnv } from "@midos/vue-ui";

@injectable()
export class RcsExtHttp extends HttpClient {
  public key = "RcsExtHttp";

  public constructor(env: VueEnv) {
    super(env.IsStaging
      ? "http://172.16.2.74:5300"
      : env.IsProduction
        ? "http://172.16.2.64:5300"
        : "http://localhost:5300"
    );
  }
}

export function useRcsExtHttp() {
  return useService(RcsExtHttp);
}
