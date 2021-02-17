import { useService } from "@midos/vue-ui";
import { Notify } from "../services/notify";

export function useNotify() {
  return useService(Notify);
}
