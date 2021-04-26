<template>
  <LogList :classes="['wcs.door']" />
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
