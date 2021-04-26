<template>
  <LogList />
</template>

<script lang="ts">
import { defineComponent } from "vue";
import LogList from "../../components/LogList.vue";

export default defineComponent({
  name: "Logs",

  components: {
    LogList
  }
});
</script>
