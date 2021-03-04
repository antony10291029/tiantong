<template>
  <div style="padding: 1.25rem; overflow: auto">
    <LogList :classes="['wcs.door']" />
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import LogList from "../../components/LogList.vue";

export default defineComponent({
  name: "AutoDoors",

  components: {
    LogList,
  }
});
</script>
