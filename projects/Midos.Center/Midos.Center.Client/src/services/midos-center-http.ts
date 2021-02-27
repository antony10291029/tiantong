import { HttpClient, injectable } from "@midos/core";
import { useService } from "@midos/vue-ui";

@injectable()
export class MidosCenterHttp extends HttpClient {
  public key = "MidosCenterHttp";

  public constructor() {
    super("http://localhost:4800");
  }
}

export function useMidosCenterHttp() {
  return useService(MidosCenterHttp);
}
