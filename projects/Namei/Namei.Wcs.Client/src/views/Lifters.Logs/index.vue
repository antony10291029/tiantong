<template>
  <LogList :classes="['wcs.lifter']" />
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
