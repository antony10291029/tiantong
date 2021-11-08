import { httpClient } from "../services";

enum PlcStateType {
  uint16 = "uint16",
  int32 = "int32",
  bool = "bool",
  string = "string",
}

enum PlcStateCommand {
  get = "get",
  set = "set",
}

interface PlcStateConfig {
  id: number;
  plc_id: number;
  name: string;
  number: string;
  type: PlcStateType;
  address: string;
  length: number;
  is_heartbeat: boolean;
  heartbeat_interval: number;
  heartbeat_max_value: number;
  is_collect: boolean;
  collect_interval: number;
  is_read_log_on: boolean;
  is_write_log_on: boolean;
}

const PlcStateConfigContext = {
  toArray: (plcId: number) => httpClient.post<PlcStateConfig[]>("/plcs/states/all", { plc_id: plcId }),
};

export {
  PlcStateType,
  PlcStateCommand,
  PlcStateConfig,
  PlcStateConfigContext,
};
