<template>
  <div style="padding: 1.25rem;">
    <LogList :classes="['wcs.lifter']" />
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import LogList from "../../components/LogList.vue";

export default defineComponent({
  name: "Lifters",

  components: {
    LogList
  }
});
</script>
