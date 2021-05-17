import { Entity } from "@midos/seed-work";

export class Route implements Entity {
  public id = 0;

  public name = "";

  public path = "";

  public endpointPath = "";

  public endpointId = 0;
}
