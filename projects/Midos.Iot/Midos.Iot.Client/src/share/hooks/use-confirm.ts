import { getCurrentInstance } from "vue";
import { IConfirm } from "../components/Confirm";

export function useConfirm() {
  const confirm = getCurrentInstance()?.appContext.config.globalProperties.$confirm;

  return confirm as IConfirm;
}
