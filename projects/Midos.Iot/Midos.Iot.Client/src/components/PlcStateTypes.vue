<template>
  <div>
    <div
      @click="$emit('update:value', dataType.value)"
      v-for="(dataType, key) in dataTypes" :key="key"
      class="is-bordered is-flex is-vcentered"
      style="padding: 0.5rem; cursor: pointer"
      :style="key !== 0 && 'border-top: none'"
    >
      <Radio :value="value === dataType.value">
        {{dataType.text}}
      </Radio>
    </div>
  </div>
</template>

<script lang="ts">
import { computed, defineComponent } from "vue";
import { PlcStateType } from "../entities";

export default defineComponent({
  name: "PlcStateTypes",

  props: {
    value: {
      type: String,
      requried: true
    },

    length: {
      type: Number,
      required: true
    }
  },

  setup(props) {
    const dataTypes = computed(() => ([
      {
        value: PlcStateType.uint16,
        text: "整数（16 位 - 无符号）"
      },
      {
        value: PlcStateType.int32,
        text: "整数（32 位）"
      },
      {
        value: PlcStateType.bool,
        text: "布尔（1 位）"
      },
      {
        value: PlcStateType.string,
        text: `字符串（${props.length * 2} 位 - ASCII）`
      },
    ]));

    return {
      dataTypes
    };
  }
});
</script>
