<template>
  <div style="padding: 1.25rem;">
    <LogList :search="search" />
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import LogList from "@/components/LogList.vue";

export default defineComponent({
  name: "Lifters",

  components: {
    LogList
  },

  setup() {
    const search = ref(["lifter"]);

    return {
      search
    };
  }
});
</script>
