import { useService } from "@midos/vue-ui";
import { Confirm } from "../services/confirm";

export function useConfirm() {
  return useService(Confirm);
}
