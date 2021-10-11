<template>
  <textarea
    type="text"
    class="
      px-3 py-2 w-full
      text-md relative rounded
      outline-none
      border border-dark-700 focus:border-link-500
      focus:bg-dark-700
      ring-2 ring-transparent focus:ring-opacity-30 focus:ring-link-500
      bg-dark-900 hover:bg-dark-800
    "
    :value="value"
    rows="3"
    @input="handleInput"
  ></textarea>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  props: {
    value: {
      type: String,
      default: ""
    }
  },

  emits: [
    "update:value"
  ],

  setup(props, { emit }) {
    const handleInput = (event: any) => {
      emit("update:value", event.target.value.trim());
    };

    return {
      handleInput
    };
  }
});
</script>
