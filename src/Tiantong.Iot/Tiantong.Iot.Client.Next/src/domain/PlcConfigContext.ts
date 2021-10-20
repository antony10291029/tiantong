import { httpClient } from "../services";

type PlcModel = "test"
  | "mc3e-binary"
  | "mc1e-binary"
  | "s7200smart";

interface PlcConfig {
  id: number;
  name: string;
  model: PlcModel;
  number: string;
  host: string;
  port: number;
  comment: string;
  created_at: number;
}

const PlcConfigContext = {
  addByName: (name: string) => httpClient.post("plcs/create", { name }),

  removeById: (id: number) => httpClient.post("plcs/delete", { plc_id: id }),

  update: (plc: PlcConfig) => httpClient.post("plcConfigs/update", plc),

  getById: (id: number) => httpClient.post<PlcConfig>("plcConfigs/get", { id }),

  toArray: () => httpClient.post<PlcConfig[]>("/plcs/all"),
};

export {
  PlcModel,
  PlcConfig,
  PlcConfigContext
};
