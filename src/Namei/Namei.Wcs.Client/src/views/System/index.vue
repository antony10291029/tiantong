<template>
  <div>
    <div class="box" style="margin: 1.25rem">
      <ClearLogs />
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import ClearLogs from "./ClearLogs.vue";

export default defineComponent({
  name: "System",

  components: {
    ClearLogs
  }
});
</script>
