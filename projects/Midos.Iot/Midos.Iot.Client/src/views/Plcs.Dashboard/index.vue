<template>
  <div style="padding: 1.25rem">
    <div class="box" style="padding: 0.5rem 0">
      <div>
        <QuickOperations />
      </div>

      <hr style="margin: 1.25rem">

      <div>
        <Autorun />
      </div>

      <hr style="margin: 1.25rem">

      <div>
        <ClearLogs />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import QuickOperations from "./QuickOperations.vue";
import Autorun from "./Autorun.vue";
import ClearLogs from "./ClearLogs.vue";

export default defineComponent({
  name: "PlcsDashboard",

  components: {
    Autorun,
    ClearLogs,
    QuickOperations
  }
});
</script>
