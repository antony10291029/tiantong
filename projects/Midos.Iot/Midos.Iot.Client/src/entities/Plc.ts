import DateTime from "@midos/vue-ui/src/utils/DateTime";

export enum PlcModel {
  mc3eBinary = "mc3e-binary",
  mc1eBinary = "mc1e-binary",
  s7200Smart = "s7200smart",
  test = "test",
}

export class Plc {
  id = 0

  name = ""

  number = ""

  model = PlcModel.mc3eBinary

  host = "192.168.1.1"

  port = 8000

  comment = ""

  created_at = DateTime.now
}
