import { HttpClient, HttpClientMiddleware, injectable } from "@midos/core";
import { Notify } from "./notify";

@injectable()
export class HttpNotifyMiddleware extends HttpClientMiddleware {
  public key = "http-notify-middleware";

  public constructor(private notify: Notify) {
    super();
  }

  public handle(http: HttpClient) {
    http.interceptors.response.use(
      response => {
        const status = response?.status;

        if (status === 201) {
          this.notify.success(response.data.message);
        }

        return response;
      },

      error => {
        const status = error.response?.status ?? 0;

        if (status >= 400 && status < 500) {
          this.notify.danger(error.response?.data.message);
        } else if (error.response?.status === 500) {
          this.notify.danger("非常抱歉，出现未知错误");
        }

        return error;
      }
    );
  }
}
