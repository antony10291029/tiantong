import { ref } from "vue";
import { DataMap } from "@midos/core";

function UseDataMap<TEntity = unknown>() {
  return ref(new DataMap<TEntity>());
}

export {
  UseDataMap
};
