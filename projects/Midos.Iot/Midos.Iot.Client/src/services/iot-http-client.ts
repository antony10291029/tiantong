import { HttpClient, injectable } from "@midos/core";
import { useService, Config } from "@midos/vue-ui";

@injectable()
export class IotHttpClient extends HttpClient {
  public key = "IotHttpClient";

  public constructor(config: Config) {
    super(config.IsDevelopment
      ? "http://localhost:5101"
      : "http://172.16.2.65:5101"
    );
  }
}

export function useIotHttp() {
  return useService(IotHttpClient);
}
