import { HttpClient } from "@midos/core";

export class IotHttpClient extends HttpClient {
  public key = "IotHttpClient";

  public constructor() {
    super("http://localhost:5100");
  }
}
