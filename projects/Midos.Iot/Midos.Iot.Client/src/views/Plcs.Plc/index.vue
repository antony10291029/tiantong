<template>
  <div>
    Plc: {{plc}}
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "Plc",

  props: {
    plc: Object
  }
});
</script>
