import { computed, ref, watchEffect, Ref } from "vue";
import cloneDeep from "lodash/cloneDeep";
import isEqual from "lodash/isEqual";

export function useCopy<T extends Ref<T>>(dataSource: Ref<T>) {
  const data = ref<T>({} as T);
  const isChanged = computed(() => !isEqual(data.value, dataSource.value));

  watchEffect(() => data.value = cloneDeep(dataSource.value));

  return {
    data,
    isChanged
  };
}
