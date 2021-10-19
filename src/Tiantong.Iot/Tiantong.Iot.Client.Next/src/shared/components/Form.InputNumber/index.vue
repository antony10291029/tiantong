<template>
  <Input
    :value="inputValue"
    v-bind="$attrs"
    @update:value="handleInput"
    @mounted="handleMounted"
  />
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import Input from "../Form.Input/index.vue";

export default defineComponent({
  components: {
    Input
  },

  props: {
    value: {
      type: Number,
      required: true
    }
  },

  emits: ["update:value"],

  setup(props, { emit }) {
    let el: HTMLInputElement;
    const inputValue = ref(props.value.toString());
    const handleInput = (event: any) => {
      if (props.value.toString() === el.value.toString()) {
        return;
      }

      const value = parseInt(event);

      if (value !== props.value && !Number.isNaN(value)) {
        emit("update:value", value);
      }

      if (Number.isNaN(value)) {
        el.value = props.value.toString();
      } else if (el.value.toString() !== value.toString()) {
        el.value = value.toString();
      }
    };
    const handleMounted = (event: any) => {
      el = event;
    };

    return {
      inputValue,
      handleInput,
      handleMounted
    };
  }
});
</script>
