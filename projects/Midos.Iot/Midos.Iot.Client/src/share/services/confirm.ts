import { injectable, Service } from "@midos/core";
import { VueUI } from "@midos/vue-ui";
import { IConfirmOptions } from "@/share/components/Confirm/interface";

@injectable()
export class Confirm extends Service {
  public key = "confirm";

  public constructor(private ui: VueUI) {
    super();
  }

  private get confirm() {
    return this.ui.app.config.globalProperties.$confirm;
  }

  public open(options: IConfirmOptions) {
    this.confirm.open(options);
  }
}
