export enum PlcStateType {
  bool = "bool",
  uint16 = "uint16",
  int32 = "int32",
  string = "string"
}

export class PlcState {
  id = 0

  plc_id = 0

  name = ""

  number = ""

  type: string = PlcStateType.uint16

  address = ""

  length = 8

  is_heartbeat = false

  heartbeat_interval = 1000

  heartbeat_max_value = 1000

  is_collect = false

  collect_interval = 1000

  is_read_log_on = false

  is_write_log_on = false
}
