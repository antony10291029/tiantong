<template>
  <TheInput
    :value="inputValue"
    @update:value="handleInput"
    @mounted="handleMounted"
  />
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import TheInput from "./TheInput.vue";

export default defineComponent({
  components: {
    TheInput
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
      const value = parseInt(event);

      console.log(value);

      if (!Number.isNaN(value)) {
        emit("update:value", value);
      } else {
        el.value = props.value.toString();
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
