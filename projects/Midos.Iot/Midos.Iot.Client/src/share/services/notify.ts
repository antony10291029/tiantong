import { injectable, Service } from "@midos/core";
import { VueUI } from "@midos/vue-ui";

@injectable()
export class Notify extends Service {
  public key = "notify";

  public constructor(private ui: VueUI) {
    super();
  }

  private get notify() {
    return this.ui.app.config.globalProperties.$notify;
  }

  public info(text: string, duration = 3333) {
    this.notify.open(text, "info", duration);
  }

  public link(text: string, duration = 3333) {
    this.notify.open(text, "link", duration);
  }

  public danger(text: string, duration = 3333) {
    this.notify.open(text, "danger", duration);
  }

  public success(text: string, duration = 3333) {
    this.notify.open(text, "success", duration);
  }

  public warning(text: string, duration = 3333) {
    this.notify.open(text, "warning", duration);
  }
}
