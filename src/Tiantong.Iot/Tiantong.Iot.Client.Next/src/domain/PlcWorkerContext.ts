import { httpClient } from "../services";
import { PlcStateType, PlcStateCommand } from "./PlcStateContext";

interface DebugParams {
  plcId: number,
  command: PlcStateCommand,
  address: string,
  dataType: PlcStateType,
  length: number,
  value: string,
}

const PlcWorkerContext = {
  debug(params: DebugParams) {
    const url = `/plc-workers/debug/${params.dataType}/${params.command}`;
    const postParams = {
      plc_id: params.plcId,
      address: params.address,
      length: params.length,
      value: params.value,
    };

    return httpClient.post<{ message: string }>(url, postParams);
  }
};

export {
  DebugParams,
  PlcWorkerContext,
};
