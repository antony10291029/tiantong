<template>
  <div style="padding: 1.25rem; overflow: auto">
    <LogList :search="search" />
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import LogList from "@/components/LogList.vue";

export default defineComponent({
  name: "AutoDoors",

  components: {
    LogList,
  },

  setup() {
    const search = ref(["door"]);

    return {
      search
    };
  }
});
</script>
