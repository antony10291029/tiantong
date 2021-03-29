import { injectable, Service } from "@midos/core";
import { useService } from "@midos/vue-ui";
import { MidosCenterHttp } from "./midos-center-http";

@injectable()
export class MidosCenterApi extends Service {
  public key = "midos.center.api";

  public constructor(
    private http: MidosCenterHttp
  ) {
    super();
  }

  public async getApps(klass = "default") {
    return await this.http.getDataArray("/midos/apps/search", {
      klass
    });
  }

  public async getConfigs() {
    return await this.http.getDataArray("/midos/configs");
  }
}

export function useMidosCenterApi() {
  return useService(MidosCenterApi);
}
