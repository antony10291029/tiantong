<template>
  <div
    class="
      bg-dark-800
      rounded
      w-full h-20 mb-3 p-4
      flex items-center
      hover:bg-dark-700
    "
  >
    <slot />
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({

});
</script>
