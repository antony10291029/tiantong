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

    <Input
      type="text"
      class="pl-8"
      :value="inputValue"
      @input="handleInput"
    />
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, } from "vue";
import Input from "../shared/components/Form.Input/index.vue";

export default defineComponent({
  components: {
    Input
  },

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
