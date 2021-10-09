<template>
  <div
    class="
      relative
      flex items-center
      text-dark-500
      focus-within:text-dark-300
    "
  >
    <span
      class="
        absolute px-2 z-10
      "
    >
      <i class="iconfont icon-search"></i>
    </span>

    <input
      type="text"
      class="
        w-48 px-3 py-1.5 pl-8
        text-md relative
        outline-none ring-1 ring-transparent
        focus:ring-dark-500 focus:bg-dark-700
        bg-dark-800 hover:bg-dark-700
      "
      :value="inputValue"
      @input="handleInput"
    >
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, } from "vue";

export default defineComponent({
  props: {
    value: {
      type: String,
      required: true
    }
  },

  emits: [
    "update:value"
  ],

  setup(props, { emit }) {
    const inputValue = ref(props.value);
    const handleInput = (event: any) => {
      inputValue.value = event.target.value.trim();
      emit("update:value", inputValue.value);
    };

    return {
      inputValue,
      handleInput,
    };
  }
});
</script>
