<template>
  <div style="padding: 1.25rem">
    <div class="box">
      <p class="title is-5">
        系统配置
      </p>

      <hr>

      <ClearLogs />

      <hr>

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
