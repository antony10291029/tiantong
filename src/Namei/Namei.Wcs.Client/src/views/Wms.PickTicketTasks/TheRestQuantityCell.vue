<template>
  <td>
    <span
      v-if="value"
      class="has-text-danger has-text-weight-bold"
    >
      {{value}}
    </span>
    <span v-else>
      {{value}}
    </span>
  </td>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  props: {
    value: {
      type: Number,
      required: true
    }
  }
});
</script>
