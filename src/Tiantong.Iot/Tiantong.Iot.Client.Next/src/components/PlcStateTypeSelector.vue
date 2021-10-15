<template>
  <div class="border border-dark-700 rounded">
    <div
      @click="$emit('update:value', type.value)"
      v-for="(type, key) in types" :key="key"
      class="
        px-3 py-1.5
        border-b border-dark-700
        last:border-b-0 cursor-pointer
        hover:bg-dark-800
      "
    >
      <Radio :value="value === type.value" />
      <span class="ml-3">
        {{ type.text }}
      </span>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";
import { PlcStateType } from "../domain";
import Radio from "../shared/components/Radio/index.vue";

export default defineComponent({
  components: {
    Radio
  },

  props: {
    value: {
      type: String as PropType<PlcStateType>,
      default: "uint16"
    },

    length: {
      type: Number,
      default: 4
    }
  },

  setup(props) {
    const types = [
      {
        value: "uint16",
        text: "整数（16 位 - 无符号）"
      },
      {
        value: "int32",
        text: "整数（32 位）"
      },
      {
        value: "bool",
        text: "布尔（1 位）"
      },
      {
        value: "string",
        text: `字符串（${props.length * 2} 位 - ASCII）`
      }
    ];

    return {
      types
    };
  }
});
</script>
