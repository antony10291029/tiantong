<template>
  <AsyncLoader
    :handler="getDataSource"
    class="is-flex is-flex-column"
    style="padding: 1.25rem; overflow: auto"
    v-slot="{ isLoading }"
  >
    <template v-if="!isLoading">
      <div class="columns">
        <div
          class="column is-narrow"
          v-for="id in ['1', '2', '3']" :key="id"
        >
          <Lifter :lifterId="id" :lifter="lifters[id]" :doors="doors" />
        </div>
      </div>

      <Doors :doors="doors" />
    </template>
  </AsyncLoader>
</template>

<script>
/* eslint-disable vue/no-unused-components */
import { defineComponent, ref } from "vue";
import { useWcsHttp } from "@/services/wcs-http";
import { useInterval } from "@midos/vue-ui";
import Doors from "./Doors.vue";
import Lifter from "./Lifter.vue";

export default defineComponent({
  name: "Devices",
  components: {
    Doors,
    Lifter,
  },

  setup() {
    const http = useWcsHttp();

    const lifters = ref([{
      isWorking: false,
      isAlerting: false,
      floors: [3, 2, 1, 0].map(() => ({
        isDoorOpened: false,
        isExported: false,
        isImportAllowed: false,
        isScanned: false,
      }))
    }]);
    const doors = ref([{
      id: "",
      type: "automated",
      IsOpened: false,
      taskId: "",
      count: 0,
    }]);
    const isInitialized = ref([false, false]);
    const isPending = ref(false);

    async function getLifters() {
      const result = await http.post("/lifters/states");

      lifters.value = result;
    }

    async function getDoors() {
      const result = await http.post("/doors/states");

      doors.value = result;
    }

    async function getDataSource() {
      await Promise.all([getLifters(), getDoors()]);
    }

    useInterval(getDataSource, ref(true));

    return {
      lifters,
      doors,
      isInitialized,
      isPending,
      getDataSource
    };
  }
});
</script>
