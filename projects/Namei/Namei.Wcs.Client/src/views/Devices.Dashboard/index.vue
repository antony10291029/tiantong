<template>
  <AsyncLoader
    :handler="getDataSource"
    class="is-flex is-flex-column"
    style="padding: 1.25rem; overflow: auto"
    v-slot="{ isLoading }"
  >
    <template v-if="!isLoading">
      <div class="columns is-multiline" style="flex: 1 1 0px">
        <div
          class="column is-narrow-fullhd is-half-desktop is-12-tablet"
          v-for="id in ['1', '2', '3']" :key="id"
        >
          <TheLifter :lifterId="id" :lifter="lifters[id]" :doors="doors" />
        </div>
      </div>

      <TheDoors :doors="doors" />
    </template>
  </AsyncLoader>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { useInterval } from "@midos/vue-ui";
import { useWcsHttp } from "../../services/wcs-http";
import TheDoors from "./Doors.vue";
import TheLifter from "./Lifter.vue";
import { Obj, Door, Lifter } from "./_interfaces";

export default defineComponent({
  name: "Devices",
  components: {
    TheDoors,
    TheLifter,
  },

  setup() {
    const http = useWcsHttp();

    const lifters = ref<Obj<Lifter>>({});
    const doors = ref<Obj<Door>>({});
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
