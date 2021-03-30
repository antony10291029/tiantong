import { ref } from "vue";
import { Pagination } from "@midos/core";

function usePagination<TEntity = unknown>() {
  return ref(new Pagination<TEntity>());
}

export {
  usePagination
};
