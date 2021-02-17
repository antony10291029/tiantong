<template>
  <div style="padding: 1.25rem">
    <div class="box is-paddingless">
      <div
        style="border-bottom: none"
      >
        <QuickOperations />
      </div>

      <hr style="margin: 1.25rem">

      <div>
        <Autorun />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import QuickOperations from "./QuickOperations.vue";
import Autorun from "./Autorun.vue";

export default defineComponent({
  name: "PlcsDashboard",

  components: {
    Autorun,
    QuickOperations
  }
});
</script>
