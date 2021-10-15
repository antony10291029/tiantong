<template>
  <div
    class="flex rounded"
  >
    <button
      v-for="method in methods" :key="method.value"
      :class="`
        border-box
        cursor-pointer hover:bg-dark-800 px-3 py-1.5
        border rounded-none
        first:rounded-l last:rounded-r
        ring-2 ring-transparent ring-opacity-40
        active:border-link-800
        ${method.value === value ? 'border-link-500 ring-link-500 z-10' : 'border-dark-700  hover:border-dark-600'}
      `"
      style="margin-right: -1px"
      @click="$emit('update:value', method.value)"
    >
      {{ method.text }}
    </button>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";
import { PlcStateCommand } from "../../domain";

export default defineComponent({
  props: {
    value: {
      type: String as PropType<PlcStateCommand>,
      required: true
    }
  },

  emits: [
    "update:value"
  ],

  setup() {
    return {
      methods: [
        { value: PlcStateCommand.get, text: "数据读取" },
        { value: PlcStateCommand.set, text: "数据写入" },
      ]
    };
  }
});
</script>
