import { HttpClient, injectable } from "@midos/core";
import { useService, VueEnv } from "@midos/vue-ui";

@injectable()
export class NameiWcsHttp extends HttpClient {
  public key = "NameiWcsHttp";

  public constructor(env: VueEnv) {
    super(env.IsStaging
      ? "http://172.16.2.74:5100"
      : env.IsProduction
        ? "http://172.16.2.64:5100"
        : "http://localhost:5100"
    );
  }
}

export function useWcsHttp() {
  return useService(NameiWcsHttp);
}
