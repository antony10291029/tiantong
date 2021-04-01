import { ref } from "vue";

function useQuery(pageSize = 15) {
  return ref({
    page: 1,
    pageSize,
    query: "",
  });
}

export {
  useQuery
};
