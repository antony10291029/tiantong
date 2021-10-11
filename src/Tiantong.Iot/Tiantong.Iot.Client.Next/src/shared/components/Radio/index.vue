<template>
  <i
    v-if="value"
    @click="handleUpdate(false)"
    class="
      iconfont icon-radio-checked
      cursor-pointer text-link-600
      group-hover:text-link-500
    "
  ></i>
  <i
    v-else
    @click="handleUpdate(true)"
    class="
      iconfont icon-radio-unchecked
      cursor-pointer text-dark-600
      group-hover:text-link-500
    "
  ></i>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  props: {
    value: {
      type: Boolean,
      default: false
    }
  },

  emits: [
    "update:value"
  ],

  setup(props, { emit }) {
    const handleUpdate = (value: boolean) => {
      emit("update:value", value);
    };

    return {
      handleUpdate
    };
  }
});
</script>
