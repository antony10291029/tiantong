<template>
  <textarea
    rows="4"
    type="text"
    class="
      input
      px-3 py-1.5 w-full
      relative rounded
      outline-none bg-transparent
      border border-dark-700
      ring-2 ring-transparent
    "
    :value="value"
    @input="handleInput"
    :readonly="readonly"
  ></textarea>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  props: {
    value: {
      type: String,
      default: ""
    },

    readonly: {
      type: Boolean,
      default: false
    },
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
