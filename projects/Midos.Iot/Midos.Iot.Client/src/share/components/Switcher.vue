<template>
  <div class="is-flex is-vcentered">
    <input
      :checked="value"
      type="checkbox" class="switch is-info"
    >
    <label @click="$emit('update:value', !value)"></label>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "Switcher",

  props: {
    value: {
      type: Boolean,
      default: true
    }
  }
});
</script>
