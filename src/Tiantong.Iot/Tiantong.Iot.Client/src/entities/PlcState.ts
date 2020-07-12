export class PlcState {
  id: number = 0
  plc_id: number = 0
  name: string = ''
  number: string = ''
  type: string = PlcStateType.uint16
  address: string = ''
  length: number = 8
  is_heartbeat: boolean = false
  heartbeat_interval: number = 1000
  heartbeat_max_value: number = 1000
  is_collect: boolean = false
  collect_interval: number = 1000
  is_read_log_on: boolean = false
  is_write_log_on: boolean = false
}

export enum PlcStateType {
  bool = 'bool',
  uint16 = 'uint16',
  int32 = 'int32',
  string = 'string'
}
