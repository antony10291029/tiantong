<template>
  <textarea
    rows="4"
    type="text"
    class="input"
    :value="value"
    @input="handleInput"
    :readonly="readonly"
  ></textarea>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import "../Form/input.css";

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
