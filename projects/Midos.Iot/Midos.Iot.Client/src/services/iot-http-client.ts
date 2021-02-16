import { HttpClient } from "@midos/core";
import { useService } from "@midos/vue-ui";

export class IotHttpClient extends HttpClient {
  public key = "IotHttpClient";

  public constructor() {
    super("http://localhost:5100");
  }
}

export function useIotHttp() {
  return useService(IotHttpClient);
}
