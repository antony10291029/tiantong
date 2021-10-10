import { httpClient } from "../services";

interface PlcConfig {
  id: number;
  name: string;
  model: string;
  number: string;
  host: string;
  port: number;
  comment: string;
  created_at: number;
}

const PlcConfigContext = {
  addByName: (name: string) => httpClient.post("plcs/create", { name }),

  removeById: (id: number) => httpClient.post("plcs/delete", { plc_id: id }),

  getById: (id: number) => httpClient.post<PlcConfig>("plcs/get", { id }),

  toArray: () => httpClient.post<PlcConfig[]>("/plcs/all"),
};

export {
  PlcConfig,
  PlcConfigContext
};
