<template>
  <div>
    <div
      class="box"
      style="margin: 1.25rem; padding: 1.75rem 1.25rem"
    >
      <TheProxySync />

      <hr style="margin: 1.75rem 0">

      <TheClearLogs />
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import TheClearLogs from "./ClearLogs.vue";
import TheProxySync from "./ProxySync.vue";

export default defineComponent({
  components: {
    TheClearLogs,
    TheProxySync,
  }
});
</script>
