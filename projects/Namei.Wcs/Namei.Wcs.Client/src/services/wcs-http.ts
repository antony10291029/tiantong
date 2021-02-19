import { HttpClient, injectable } from "@midos/core";
import { useService, Config } from "@midos/vue-ui";

@injectable()
export class NameiWcsHttp extends HttpClient {
  public key = "NameiWcsHttp";

  public constructor(config: Config) {
    super(config.IsProduction
      ? "http://localhost:5100"
      : "http://172.16.2.64:5100"
    );
  }
}

export function useWcsHttp() {
  return useService(NameiWcsHttp);
}
